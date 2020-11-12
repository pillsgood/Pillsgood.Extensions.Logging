using System;
using System.Drawing;
using Microsoft.Extensions.Logging;

namespace Pillsgood.Extensions.Logging
{
    public class LogLevelOptions
    {
        public LogLevelOption Trace { get; set; } = new LogLevelOption()
        {
            Name = "trce",
            Foreground = Color.Gray
        };

        public LogLevelOption Debug { get; set; } = new LogLevelOption()
        {
            Name = "dbug",
            Foreground = Color.Magenta
        };

        public LogLevelOption Information { get; set; } = new LogLevelOption()
        {
            Name = "info",
            Foreground = Color.Lime
        };

        public LogLevelOption Warning { get; set; } = new LogLevelOption()
        {
            Name = "warn",
            Foreground = Color.Orange
        };

        public LogLevelOption Error { get; set; } = new LogLevelOption()
        {
            Name = "fail",
            Foreground = Color.Red,
        };

        public LogLevelOption Critical { get; set; } = new LogLevelOption()
        {
            Name = "crit",
            Foreground = Color.White,
            Background = Color.Red
        };

        public void Configure(LogLevel logLevel, Action<LogLevelOption> configure)
        {
            if (TryGetLevelOption(logLevel, out var option))
            {
                configure(option);
            }
        }

        public bool TryGetLevelOption(LogLevel logLevel, out LogLevelOption option)
        {
            option = logLevel switch
            {
                LogLevel.Trace => Trace,
                LogLevel.Debug => Debug,
                LogLevel.Information => Information,
                LogLevel.Warning => Warning,
                LogLevel.Error => Error,
                LogLevel.Critical => Critical,
                _ => null
            };
            return option != null;
        }

        public class LogLevelOption
        {
            public string Name { get; set; }
            public Color? Foreground { get; set; }
            public Color? Background { get; set; }
        }
    }
}