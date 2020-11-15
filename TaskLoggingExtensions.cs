using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Pillsgood.Extensions.Logging
{
    public static class TaskLoggingExtensions
    {
        public static void LogExceptions(this Task task, ILogger logger)
        {
            task.ContinueWith(continuationAction =>
            {
                if (continuationAction.IsFaulted)
                {
                    logger.LogError(continuationAction.Exception, null);
                }
            });
        }
    }
}