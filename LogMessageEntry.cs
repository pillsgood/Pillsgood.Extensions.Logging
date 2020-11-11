namespace Pillsgood.Extensions.Logging
{
    internal class LogMessageEntry
    {
        public string Message { get; }
        public bool LogAsError { get; }

        public LogMessageEntry(string message, bool logAsError = false)
        {
            Message = message;
            LogAsError = logAsError;
        }
    }
}