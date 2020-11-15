using System;

namespace Pillsgood.Extensions.Logging
{
    public class DefaultAnsiConsoleFormatterOptions : AnsiConsoleFormatterOptions
    {
        public LoggerColorBehavior ColorBehavior { get; set; }
        public bool Singleline { get; set; }
        public bool WriteEventName { get; set; }
    }
}