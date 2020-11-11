namespace Pillsgood.Extensions.Logging
{
    public class DefaultConsoleFormatterOptions : ConsoleFormatterOptions
    {
        public DefaultConsoleFormatterOptions()
        {
        }

        public LoggerColorBehavior ColorBehavior { get; set; }
        public bool Singleline { get; set; }
    }
}