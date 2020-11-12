using System.Drawing;
using Microsoft.Extensions.Logging;

namespace Pillsgood.Extensions.Logging
{
    public class LogLevelConfiguration
    {
        private readonly LogLevelOptions _logLevelOptions;

        internal LogLevelConfiguration(LogLevelOptions logLevelOptions)
        {
            _logLevelOptions = logLevelOptions;
        }

        public LogLevelConfiguration Set(LogLevel logLevel, string name, Color? foreground = null,
            Color? background = null)
        {
            _logLevelOptions.Configure(logLevel, option =>
            {
                option.Name = name;
                option.Foreground = foreground;
                option.Background = background;
            });

            return this;
        }
    }
}