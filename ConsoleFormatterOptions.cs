namespace Pillsgood.Extensions.Logging
{
    public class ConsoleFormatterOptions
    {
        public ConsoleFormatterOptions()
        {
        }

        public bool IncludeScopes { get; set; }
        public string TimestampFormat { get; set; }
        public bool UseUtcTimestamp { get; set; }
    }
}