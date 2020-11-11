using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Pillsgood.Extensions.Logging
{
    internal class ConsoleLogger : ILogger
    {
        private readonly string _name;

        private readonly ConsoleLoggerProcessor _queueProcessor;

        internal ConsoleLogger(string name, ConsoleLoggerProcessor loggerProcessor)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _queueProcessor = loggerProcessor;
        }

        internal IConsoleFormatter Formatter { get; set; }
        internal IExternalScopeProvider ScopeProvider { get; set; }
        internal ConsoleLoggerOptions Options { get; set; }

        [ThreadStatic] private static StringWriter _stringWriter;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            _stringWriter ??= new StringWriter();
            var logEntry = new LogEntry<TState>(logLevel, _name, eventId, state, exception, formatter);
            Formatter.Write(in logEntry, ScopeProvider, _stringWriter);

            var sb = _stringWriter.GetStringBuilder();
            if (sb.Length == 0) return;
            var computedAnsiString = sb.ToString();
            sb.Clear();
            if (sb.Capacity > 1024)
            {
                sb.Capacity = 1024;
            }

            _queueProcessor.EnqueueMessage(new LogMessageEntry(computedAnsiString,
                logLevel >= Options.LogToStandardErrorThreshold));
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? NullScope.Instance;
    }
}