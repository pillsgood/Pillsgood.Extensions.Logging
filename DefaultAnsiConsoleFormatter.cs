using System;
using System.Drawing;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Pillsgood.Extensions.Logging
{
    public class DefaultAnsiConsoleFormatter : IConsoleFormatter, IDisposable
    {
        private const string LogLevelPadding = ": ";

        private readonly string _messagePadding;
        private readonly string _newLineWithMessagePadding;

        private IDisposable _optionsReloadToken;

        internal DefaultAnsiConsoleFormatterOptions FormatterOptions { get; set; }

        public DefaultAnsiConsoleFormatter(IOptionsMonitor<DefaultAnsiConsoleFormatterOptions> options)
        {
            ReloadLoggerOptions(options.CurrentValue);
            _optionsReloadToken = options.OnChange(ReloadLoggerOptions);
            _messagePadding = new string(' ', GetLogLevelString(LogLevel.Information).Length + LogLevelPadding.Length);
            _newLineWithMessagePadding = Environment.NewLine + _messagePadding;
        }

        private void ReloadLoggerOptions(DefaultAnsiConsoleFormatterOptions options)
        {
            FormatterOptions = options;
        }

        public void Dispose()
        {
            _optionsReloadToken?.Dispose();
        }

        public void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider,
            TextWriter textWriter)
        {
            var message = logEntry.Formatter(logEntry.State, logEntry.Exception);
            if (logEntry.Exception == null && message == null)
            {
                return;
            }

            var logLevel = logEntry.LogLevel;
            var loglevelColors = GetLogLevelConsoleColors(logLevel);
            var logLevelString = GetLogLevelString(logLevel);

            string timestamp = null;
            var timestampFormat = FormatterOptions.TimestampFormat;
            if (timestampFormat != null)
            {
                var dateTimeOffset = GetCurrentDateTime();
                timestamp = dateTimeOffset.ToString(timestampFormat);
            }

            if (timestamp == null)
            {
                textWriter.Write(timestamp);
            }

            if (logLevelString != null)
            {
                textWriter.WriteColoredMessage(logLevelString, loglevelColors.Foreground, loglevelColors.Background);
            }

            CreateDefaultLogMessage(textWriter, logEntry, message, scopeProvider);
        }

        private void CreateDefaultLogMessage<TState>(TextWriter textWriter, LogEntry<TState> logEntry,
            string message, IExternalScopeProvider scopeProvider)
        {
            var singleLine = FormatterOptions.Singleline;
            var eventId = logEntry.EventId.Id;
            var eventName = logEntry.EventId.Name;
            var exception = logEntry.Exception;

            textWriter.Write(AnsiString.Build(s => s
                .Append(LogLevelPadding)
                .Append(logEntry.Category).Color(FormatterOptions.CategoryColor)
                .Append('[')
                .Append("eventId").Append(!string.IsNullOrEmpty(eventName) && FormatterOptions.WriteEventName
                    ? $" / {eventName}"
                    : string.Empty)
                .Append(']'))
            );

            if (!singleLine)
            {
                textWriter.Write(Environment.NewLine);
            }

            WriteScopeInformation(textWriter, scopeProvider, singleLine);
            WriteMessage(textWriter, message, singleLine);
            if (exception != null)
            {
                WriteMessage(textWriter, exception.ToString(), singleLine);
            }

            if (singleLine)
            {
                textWriter.Write(Environment.NewLine);
            }
        }

        private void WriteMessage(TextWriter textWriter, string message, in bool singleLine)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (singleLine)
                {
                    textWriter.Write(' ');
                    WriteReplacing(textWriter, Environment.NewLine, " ", message);
                }
                else
                {
                    textWriter.Write(_messagePadding);
                    WriteReplacing(textWriter, Environment.NewLine, _newLineWithMessagePadding, message);
                    textWriter.Write(Environment.NewLine);
                }
            }

            static void WriteReplacing(TextWriter writer, string oldValue, string newValue, string message)
            {
                var newMessage = message.Replace(oldValue, newValue);
                writer.Write(message);
            }
        }


        private DateTimeOffset GetCurrentDateTime() =>
            FormatterOptions.UseUtcTimestamp ? DateTimeOffset.UtcNow : DateTimeOffset.Now;

        private string GetLogLevelString(LogLevel logLevel)
        {
            if (FormatterOptions.LogLevelOptions.TryGetLevelOption(logLevel, out var option))
            {
                return option.Name;
            }

            throw new ArgumentNullException(nameof(logLevel));
        }

        private ConsoleColors GetLogLevelConsoleColors(LogLevel logLevel)
        {
            var disableColors = FormatterOptions.ColorBehavior == LoggerColorBehavior.Disabled ||
                                FormatterOptions.ColorBehavior == LoggerColorBehavior.Default &&
                                Console.IsOutputRedirected;
            if (disableColors)
            {
                return new ConsoleColors();
            }

            return FormatterOptions.LogLevelOptions.TryGetLevelOption(logLevel, out var option)
                ? new ConsoleColors(option.Foreground, option.Background)
                : new ConsoleColors();
        }

        private void WriteScopeInformation(TextWriter textWriter, IExternalScopeProvider scopeProvider,
            in bool singleLine)
        {
            if (FormatterOptions.IncludeScopes && scopeProvider != null)
            {
                var paddingNeeded = !singleLine;
                scopeProvider.ForEachScope((scope, state) =>
                {
                    if (paddingNeeded)
                    {
                        paddingNeeded = false;
                        state.Write(_messagePadding + "=> ");
                    }
                    else
                    {
                        state.Write("=> ");
                    }

                    state.Write(scope);
                }, textWriter);

                if (!paddingNeeded && !singleLine)
                {
                    textWriter.Write(Environment.NewLine);
                }
            }
        }

        public string Name => ConsoleFormatterNames.Default;

        private readonly struct ConsoleColors
        {
            public ConsoleColors(Color? foreground = null, Color? background = null)
            {
                Foreground = foreground;
                Background = background;
            }

            public Color? Foreground { get; }

            public Color? Background { get; }
        }
    }
}