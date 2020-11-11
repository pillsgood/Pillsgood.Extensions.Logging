using System;
using System.Drawing;
using Microsoft.Extensions.Logging;

namespace Pillsgood.Extensions.Logging
{
    public class DefaultConsoleFormatterOptions : ConsoleFormatterOptions
    {
        public LoggerColorBehavior ColorBehavior { get; set; }
        public bool Singleline { get; set; }
        internal LogLevelOptions LogLevelOptions { get; set; } = new LogLevelOptions();

        public void ConfigureLogLevel(Action<LogLevelConfiguration> configure)
        {
            configure(new LogLevelConfiguration(LogLevelOptions));
        }
    }

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
            _logLevelOptions.Map[logLevel] = new LogLevelOptions.LogLevelOption
            {
                Name = name,
                Foreground = foreground,
                Background = background
            };
            return this;
        }
    }
}