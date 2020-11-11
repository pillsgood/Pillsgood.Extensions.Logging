using Microsoft.Extensions.Logging;

namespace Pillsgood.Extensions.Logging
{
    public class ConsoleLoggerOptions
    {
        public string FormatterName { get; set; } = ConsoleFormatterNames.Default;
        public LogLevel LogToStandardErrorThreshold { get; set; } = LogLevel.None;
    }
}