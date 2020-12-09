using Microsoft.Extensions.Logging;

namespace Pillsgood.Extensions.Logging
{
    public class AnsiConsoleLoggerOptions
    {
        public string FormatterName { get; set; } = ConsoleFormatterNames.Default;
        public LogLevel LogToStandardErrorThreshold { get; set; } = LogLevel.None;
        public string[] UnsupportedProcesses { get; set; } = { "winpty-agent", };
        public bool Force4BitColor { get; set; } = false;
        internal bool AddConsoleWriter { get; set; } = false;
        public int TimeoutDuration { get; set; }
    }
}