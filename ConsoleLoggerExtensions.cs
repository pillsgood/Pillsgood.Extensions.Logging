using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Pillsgood.Extensions.Logging
{
    public static class ConsoleLoggerExtensions
    {
        public static ILoggingBuilder AddConsole(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.AddConsoleFormatter<DefaultConsoleFormatter, DefaultConsoleFormatterOptions>();
            //TODO : more console formatters

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
            LoggerProviderOptions
                .RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddConsoleFormatter<TFormatter, TOptions>(this ILoggingBuilder builder)
            where TOptions : ConsoleFormatterOptions
            where TFormatter : class, IConsoleFormatter
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConsoleFormatter, TFormatter>());
            builder.Services.TryAddEnumerable(ServiceDescriptor
                .Singleton<IConfigureOptions<TOptions>, ConsoleLoggerFormatterConfigureOptions<TFormatter, TOptions>
                >());
            builder.Services.TryAddEnumerable(ServiceDescriptor
                .Singleton<IOptionsChangeTokenSource<TOptions>,
                    ConsoleLoggerFormatterOptionsChangeTokenSource<TFormatter, TOptions>>());
            return builder;
        }
    }

    internal class
        ConsoleLoggerFormatterConfigureOptions<TFormatter, TOptions> : ConfigureFromConfigurationOptions<TOptions>
        where TOptions : ConsoleFormatterOptions
        where TFormatter : class, IConsoleFormatter

    {
        public ConsoleLoggerFormatterConfigureOptions(
            ILoggerProviderConfiguration<ConsoleLoggerProvider> providerConfiguration) : base(
            providerConfiguration.Configuration.GetSection("FormatterOptions"))
        {
        }
    }

    internal class
        ConsoleLoggerFormatterOptionsChangeTokenSource<TFormatter, TOptions> : ConfigurationChangeTokenSource<TOptions>
        where TOptions : ConsoleFormatterOptions
        where TFormatter : class, IConsoleFormatter
    {
        public ConsoleLoggerFormatterOptionsChangeTokenSource(
            ILoggerProviderConfiguration<ConsoleLoggerProvider> providerConfiguration)
            : base(providerConfiguration.Configuration.GetSection("FormatterOptions"))
        {
        }
    }
}