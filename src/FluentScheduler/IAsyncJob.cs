﻿#if NETSTANDARD2_1
namespace Xunet.FluentScheduler
{
    using System.Threading.Tasks;

    /// <summary>
    /// Some asynchrounous work to be done.
    /// Make sure there's a parameterless constructor (avoid System.MissingMethodException).
    /// </summary>
    public interface IAsyncJob : IJob
    {
        /// <summary>
        /// Executes the job.
        /// </summary>
        Task ExecuteAsync();

        /// <summary>
        /// Executes the job, synchronously.
        /// </summary>
        void IJob.Execute()
        {
            ExecuteAsync().Wait();
        }
    }
}
#endif
