namespace Xunet.FluentScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunet.TimeCrontab;

    /// <summary>
    /// A job schedule.
    /// </summary>
    public class Schedule
    {
        /// <summary>
        /// Date and time of the next run of this job schedule.
        /// </summary>
        public DateTime NextRun { get; internal set; }

        /// <summary>
        /// Name of this job schedule.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Flag indicating if this job schedule is disabled.
        /// </summary>
        public bool Disabled { get; private set; }

        internal List<Action> Jobs { get; private set; }

        internal Func<DateTime, DateTime> CalculateNextRun { get; set; }

        internal TimeSpan DelayRunFor { get; set; }

        internal ICollection<Schedule> AdditionalSchedules { get; set; }

        internal Schedule Parent { get; set; }

        internal bool PendingRunOnce { get; set; }

        internal object Reentrant { get; set; }

        /// <summary>
        /// Schedules a new job in the registry.
        /// </summary>
        /// <param name="action">Job to schedule.</param>
        public Schedule(Action action) : this(new[] { action }) { }

        /// <summary>
        /// Schedules a new job in the registry.
        /// </summary>
        /// <param name="actions">Jobs to schedule</param>
        public Schedule(IEnumerable<Action> actions)
        {
            Disabled = false;
            Jobs = actions.ToList();
            AdditionalSchedules = new List<Schedule>();
            PendingRunOnce = false;
            Reentrant = null;
        }

        /// <summary>
        /// Executes the job regardless its schedule.
        /// </summary>
        public void Execute()
        {
            JobManager.RunJob(this);
        }

        /// <summary>
        /// Schedules another job to be run with this schedule.
        /// </summary>
        /// <param name="job">Job to run.</param>
        public Schedule AndThen(Action job)
        {
            if (job == null)
                throw new ArgumentNullException(nameof(job));

            Jobs.Add(job);
            return this;
        }

        /// <summary>
        /// Schedules another job to be run with this schedule.
        /// </summary>
        /// <param name="job">Job to run.</param>
        public Schedule AndThen(IJob job)
        {
            if (job == null)
                throw new ArgumentNullException(nameof(job));

            Jobs.Add(JobManager.GetJobAction(job));
            return this;
        }

        /// <summary>
        /// Schedules another job to be run with this schedule.
        /// </summary>
        /// <param name="job">Job to run.</param>
        public Schedule AndThen(Func<IJob> job)
        {
            if (job == null)
                throw new ArgumentNullException(nameof(job));

            Jobs.Add(JobManager.GetJobAction(job));
            return this;
        }


        /// <summary>
        /// Schedules another job to be run with this schedule.
        /// </summary>
        /// <typeparam name="T">Job to run.</typeparam>
        public Schedule AndThen<T>() where T : IJob
        {
            Jobs.Add(JobManager.GetJobAction<T>());
            return this;
        }

        /// <summary>
        /// Runs the job now.
        /// </summary>
        public SpecificTimeUnit ToRunNow()
        {
            return new SpecificTimeUnit(this);
        }

        /// <summary>
        /// Runs the job according to the given interval.
        /// </summary>
        /// <param name="interval">Interval to wait.</param>
        public TimeUnit ToRunEvery(int interval)
        {
            return new TimeUnit(this, interval);
        }

        /// <summary>
        /// 通过Cron表达式执行定时任务
        /// </summary>
        /// <param name="cron">Cron表达式</param>
        /// <param name="format">Cron表达式格式化类型</param>
        public void ToRunEveryWithCron(string cron, CronFormat format = CronFormat.Default)
        {
            CalculateNextRun = x =>
            {
                var cronStringFormat = format switch
                {
                    CronFormat.Default => CronStringFormat.Default,
                    CronFormat.WithYears => CronStringFormat.WithYears,
                    CronFormat.WithSeconds => CronStringFormat.WithSeconds,
                    CronFormat.WithSecondsAndYears => CronStringFormat.WithSecondsAndYears,
                    _ => CronStringFormat.Default
                };

                var crontab = Crontab.Parse(cron, cronStringFormat);

                var nextRun = crontab.GetNextOccurrence(JobManager.Now);

                return nextRun;
            };
        }

        /// <summary>
        /// Runs the job once after the given interval.
        /// </summary>
        /// <param name="interval">Interval to wait.</param>
        public TimeUnit ToRunOnceIn(int interval)
        {
            PendingRunOnce = true;
            return new TimeUnit(this, interval);
        }

        /// <summary>
        /// Runs the job once at the given time.
        /// </summary>
        /// <param name="hours">The hours (0 through 23).</param>
        /// <param name="minutes">The minutes (0 through 59).</param>
        public SpecificTimeUnit ToRunOnceAt(int hours, int minutes)
        {
            var dateTime =
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hours, minutes, 0);

            return ToRunOnceAt(dateTime < JobManager.Now ? dateTime.AddDays(1) : dateTime);
        }

        /// <summary>
        /// Runs the job once at the given time.
        /// </summary>
        /// <param name="time">The time to run.</param>
        public SpecificTimeUnit ToRunOnceAt(DateTime time)
        {
            CalculateNextRun = x => (DelayRunFor > TimeSpan.Zero ? time.Add(DelayRunFor) : time);
            PendingRunOnce = true;

            return new SpecificTimeUnit(this);
        }

        /// <summary>
        /// Assigns a name to this job schedule.
        /// </summary>
        /// <param name="name">Name to assign</param>
        public Schedule WithName(string name)
        {
            Name = name;
            return this;
        }

        /// <summary>
        /// Sets this job schedule as non reentrant.
        /// </summary>
        public Schedule NonReentrant()
        {
            Reentrant ??= new object();
            return this;
        }

        /// <summary>
        /// Disables this job schedule.
        /// </summary>
        public void Disable()
        {
            Disabled = true;
        }

        /// <summary>
        /// Enables this job schedule.
        /// </summary>
        public void Enable()
        {
            Disabled = false;
        }
    }

    /// <summary>
    /// Cron表达式格式化类型
    /// </summary>
    public enum CronFormat
    {
        /// <summary>
        /// 默认格式
        /// </summary>
        /// <remarks>书写顺序：分 时 日 月 周</remarks>
        Default = 0,

        /// <summary>
        /// 带年份格式
        /// </summary>
        /// <remarks>书写顺序：分 时 日 月 周 年</remarks>
        WithYears = 1,

        /// <summary>
        /// 带秒格式
        /// </summary>
        /// <remarks>书写顺序：秒 分 时 日 月 周</remarks>
        WithSeconds = 2,

        /// <summary>
        /// 带秒和年格式
        /// </summary>
        /// <remarks>书写顺序：秒 分 时 日 月 周 年</remarks>
        WithSecondsAndYears = 3
    }
}
