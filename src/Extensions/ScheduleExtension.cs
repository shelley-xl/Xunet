// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

namespace Xunet.FluentScheduler;

using TimeCrontab;

#region FluentScheduler扩展类
/// <summary>
/// FluentScheduler扩展类
/// </summary>
public static class ScheduleExtension
{
    /// <summary>
    /// 通过Cron表达式执行定时任务
    /// </summary>
    /// <param name="cron">Cron表达式</param>
    /// <param name="format">Cron表达式格式化类型</param>
    public static void ToRunWithCron(this Schedule schedule, string cron, CronFormat format = CronFormat.Default)
    {
        schedule.CalculateNextRun = x =>
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
}
#endregion

#region Cron表达式格式化类型
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
#endregion
