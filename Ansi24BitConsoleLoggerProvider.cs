using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pillsgood.Extensions.Logging
{
    [ProviderAlias("Console")]
    internal class Ansi24BitConsoleLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly IOptionsMonitor<Ansi24BitConsoleLoggerOptions> _options;
        private readonly ConcurrentDictionary<string, Ansi24BitConsoleLogger> _loggers;
        private ConcurrentDictionary<string, IConsoleFormatter> _formatters;
        private readonly Ansi24BitConsoleLoggerProcessor _messageQueue;

        private IDisposable _optionsReloadToken;
        private IExternalScopeProvider _scopeProvider = NullExternalScopeProvider.Instance;

        public Ansi24BitConsoleLoggerProvider(IOptionsMonitor<Ansi24BitConsoleLoggerOptions> options) : this(options,
            Enumerable.Empty<IConsoleFormatter>())
        {
        }


        public Ansi24BitConsoleLoggerProvider(IOptionsMonitor<Ansi24BitConsoleLoggerOptions> options,
            IEnumerable<IConsoleFormatter> formatters)
        {
            _options = options;
            _loggers = new ConcurrentDictionary<string, Ansi24BitConsoleLogger>();
            SetFormatters(formatters);
            ReloadLoggerOptions(options.CurrentValue);
            _optionsReloadToken = _options.OnChange(ReloadLoggerOptions);
            _messageQueue = new Ansi24BitConsoleLoggerProcessor();
            if (!Is4BitColorMode() && DoesConsoleSupportAnsi())
            {
                _messageQueue.Console = new AnsiLogConsole();
                _messageQueue.ErrorConsole = new AnsiLogConsole(true);
            }
            else
            {
                Console.WriteLine("ONLY 4 BIT");
                _messageQueue.Console = new AnsiParsingLogConsole();
                _messageQueue.ErrorConsole = new AnsiParsingLogConsole(true);
            }
        }

        private bool DoesConsoleSupportAnsi()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return true;
            }

            var stdOutHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.STD_OUTPUT_HANDLE);
            if (!Interop.Kernel32.GetConsoleMode(stdOutHandle, out var consoleMode))
            {
                return false;
            }

            consoleMode |= Interop.Kernel32.ENABLE_VIRTUAL_TERMINAL_PROCESSING;

            if (!Interop.Kernel32.SetConsoleMode(stdOutHandle, consoleMode))
            {
                return false;
            }

            return (consoleMode & Interop.Kernel32.ENABLE_VIRTUAL_TERMINAL_PROCESSING) ==
                   Interop.Kernel32.ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        }

        private bool Is4BitColorMode()
        {
            if (_options.CurrentValue.Force4BitColor)
            {
                return true;
            }

            var process = Process.GetCurrentProcess();
            while (process != null)
            {
                if (_options.CurrentValue.UnsupportedProcesses.Contains(process.ProcessName,
                    StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }

                process = process.Parent();
            }

            return false;
        }

        private void SetFormatters(IEnumerable<IConsoleFormatter> formatters = null)
        {
            _formatters = new ConcurrentDictionary<string, IConsoleFormatter>(StringComparer.OrdinalIgnoreCase);
            formatters = formatters as IConsoleFormatter[] ?? formatters?.ToArray();
            if (formatters?.Any() != true)
            {
                var defaultMonitor =
                    new FormatterOptionsMonitor<DefaultConsoleFormatterOptions>(new DefaultConsoleFormatterOptions());
                _formatters.GetOrAdd(ConsoleFormatterNames.Default,
                    formatterName => new DefaultConsoleFormatter(defaultMonitor));
                // TODO: More Default formatters
            }
            else
            {
                foreach (var formatter in formatters)
                {
                    _formatters.GetOrAdd(formatter.Name, formatterName => formatter);
                }
            }
        }

        private void ReloadLoggerOptions(Ansi24BitConsoleLoggerOptions options)
        {
            if (options.FormatterName == null || !_formatters.TryGetValue(options.FormatterName, out var logFormatter))
            {
                throw new ArgumentNullException(nameof(options.FormatterName));
            }


            foreach (var (_, value) in _loggers)
            {
                value.Options = options;
                value.Formatter = logFormatter;
            }
        }

        public ILogger CreateLogger(string name)
        {
            if (_options.CurrentValue.FormatterName == null ||
                !_formatters.TryGetValue(_options.CurrentValue.FormatterName, out var logFormatter))
            {
                throw new ArgumentNullException(nameof(_options.CurrentValue.FormatterName));
            }

            return _loggers.GetOrAdd(name, loggerName => new Ansi24BitConsoleLogger(name, _messageQueue)
            {
                Options = _options.CurrentValue,
                ScopeProvider = _scopeProvider,
                Formatter = logFormatter
            });
        }

        public void Dispose()
        {
            _optionsReloadToken?.Dispose();
            _messageQueue.Dispose();
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
            foreach (var logger in _loggers)
            {
                logger.Value.ScopeProvider = _scopeProvider;
            }
        }
    }
}