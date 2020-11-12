using System;
using Microsoft.Extensions.Options;

namespace Pillsgood.Extensions.Logging
{
    internal class FormatterOptionsMonitor<TOptions> : IOptionsMonitor<TOptions>
        where TOptions : AnsiConsoleFormatterOptions
    {
        private TOptions _options;

        public FormatterOptionsMonitor(TOptions options)
        {
            _options = options;
        }

        public TOptions Get(string name) => _options;

        public IDisposable OnChange(Action<TOptions, string> listener) => null;

        public TOptions CurrentValue => _options;
    }
}