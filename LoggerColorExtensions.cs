using System;
using Microsoft.Extensions.Logging;

namespace Pillsgood.Extensions.Logging
{
    public static class LoggerColorExtensions
    {
        public static void LogTrace(this ILogger logger, Action<ColorMessageBuilder> message)
        {
            var builder = new ColorMessageBuilder();
            message(builder);
            logger.LogTrace(builder.Message);
        }

        public static void LogDebug(this ILogger logger, Action<ColorMessageBuilder> message)
        {
            var builder = new ColorMessageBuilder();
            message(builder);
            logger.LogDebug(builder.Message);
        }

        public static void LogInformation(this ILogger logger, Action<ColorMessageBuilder> message)
        {
            var builder = new ColorMessageBuilder();
            message(builder);
            logger.LogInformation(builder.Message);
        }

        public static void LogWarning(this ILogger logger, Action<ColorMessageBuilder> message)
        {
            var builder = new ColorMessageBuilder();
            message(builder);
            logger.LogWarning(builder.Message);
        }

        public static void LogError(this ILogger logger, Action<ColorMessageBuilder> message)
        {
            var builder = new ColorMessageBuilder();
            message(builder);
            logger.LogError(builder.Message);
        }

        public static void LogCritical(this ILogger logger, Action<ColorMessageBuilder> message)
        {
            var builder = new ColorMessageBuilder();
            message(builder);
            logger.LogCritical(builder.Message);
        }
    }
}