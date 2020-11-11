using System.Collections.Generic;
using System.Drawing;
using Microsoft.Extensions.Logging;

namespace Pillsgood.Extensions.Logging
{
    public class LogLevelOptions
    {
        public LogLevelOption Trace
        {
            get => Map[LogLevel.Trace];
            set => Map[LogLevel.Trace] = value;
        }

        public LogLevelOption Debug
        {
            get => Map[LogLevel.Debug];
            set => Map[LogLevel.Debug] = value;
        }

        public LogLevelOption Information
        {
            get => Map[LogLevel.Information];
            set => Map[LogLevel.Information] = value;
        }

        public LogLevelOption Warning
        {
            get => Map[LogLevel.Warning];
            set => Map[LogLevel.Warning] = value;
        }

        public LogLevelOption Error
        {
            get => Map[LogLevel.Error];
            set => Map[LogLevel.Error] = value;
        }

        public LogLevelOption Critical
        {
            get => Map[LogLevel.Critical];
            set => Map[LogLevel.Critical] = value;
        }

        internal readonly Dictionary<LogLevel, LogLevelOption> Map =
            new Dictionary<LogLevel, LogLevelOption>
            {
                {
                    LogLevel.Trace, new LogLevelOption
                    {
                        Name = "trce",
                        Foreground = Color.Gray
                    }
                },
                {
                    LogLevel.Debug, new LogLevelOption
                    {
                        Name = "dbug",
                        Foreground = Color.Magenta
                    }
                },
                {
                    LogLevel.Information, new LogLevelOption
                    {
                        Name = "info",
                        Foreground = Color.Lime
                    }
                },
                {
                    LogLevel.Warning, new LogLevelOption
                    {
                        Name = "warn",
                        Foreground = Color.Orange
                    }
                },
                {
                    LogLevel.Error, new LogLevelOption
                    {
                        Name = "fail",
                        Foreground = Color.Red,
                    }
                },
                {
                    LogLevel.Critical, new LogLevelOption
                    {
                        Name = "crit",
                        Foreground = Color.White,
                        Background = Color.Red
                    }
                }
            };

        public class LogLevelOption
        {
            internal LogLevelOption()
            {
            }

            public string Name { get; set; }
            public Color? Foreground { get; set; }
            public Color? Background { get; set; }
        }
    }
}