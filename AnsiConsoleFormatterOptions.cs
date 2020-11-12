using System;
using System.Drawing;

namespace Pillsgood.Extensions.Logging
{
    public class AnsiConsoleFormatterOptions
    {
        protected AnsiConsoleFormatterOptions()
        {
        }

        public bool IncludeScopes { get; set; }
        public string TimestampFormat { get; set; }
        public bool UseUtcTimestamp { get; set; }
        
        public LogLevelOptions LogLevelOptions { get; set; } = new LogLevelOptions();

        public Color? CategoryColor { get; set; } = Color.Cyan;

        public void ConfigureLogLevel(Action<LogLevelConfiguration> configure)
        {
            configure(new LogLevelConfiguration(LogLevelOptions));
        }
    }
}