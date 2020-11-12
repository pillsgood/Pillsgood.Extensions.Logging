using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Pillsgood.Extensions.Logging
{
    public static class AnsiConsoleLoggerExtensions
    {
        public static ILoggingBuilder AddAnsiConsole(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.AddAnsiConsoleFormatter<DefaultAnsiConsoleFormatter, DefaultAnsiConsoleFormatterOptions>();
            //TODO : more console formatters

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, AnsiConsoleLoggerProvider>());
            LoggerProviderOptions
                .RegisterProviderOptions<AnsiConsoleLoggerOptions, AnsiConsoleLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddAnsiConsole(this ILoggingBuilder builder, Action<AnsiConsoleLoggerOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddAnsiConsole();
            builder.Services.Configure(configure);
            return builder;
        }

        public static ILoggingBuilder AddDefaultAnsiConsole(this ILoggingBuilder builder) =>
            builder.AddFormatterWithName(ConsoleFormatterNames.Default);
        public static ILoggingBuilder AddDefaultAnsiConsole(this ILoggingBuilder builder,
            Action<DefaultAnsiConsoleFormatterOptions> configure) =>
            builder.AddAnsi24BitConsoleWithFormatter(ConsoleFormatterNames.Default, configure);

        internal static ILoggingBuilder AddAnsi24BitConsoleWithFormatter<TOptions>(this ILoggingBuilder builder, string name,
            Action<TOptions> configure) where TOptions : AnsiConsoleFormatterOptions
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddFormatterWithName(name);
            builder.Services.Configure(configure);
            return builder;
        }

        private static ILoggingBuilder AddFormatterWithName(this ILoggingBuilder builder, string name) =>
            builder.AddAnsiConsole(options => options.FormatterName = name);

        public static ILoggingBuilder AddAnsiConsoleFormatter<TFormatter, TOptions>(this ILoggingBuilder builder)
            where TOptions : AnsiConsoleFormatterOptions
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

        public static ILoggingBuilder AddAnsiConsoleFormatter<TFormatter, TOptions>(this ILoggingBuilder builder,
            Action<TOptions> configure)
            where TOptions : AnsiConsoleFormatterOptions
            where TFormatter : class, IConsoleFormatter
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddAnsiConsoleFormatter<TFormatter, TOptions>();
            builder.Services.Configure(configure);
            return builder;
        }
    }

    internal class
        ConsoleLoggerFormatterConfigureOptions<TFormatter, TOptions> : ConfigureFromConfigurationOptions<TOptions>
        where TOptions : AnsiConsoleFormatterOptions
        where TFormatter : class, IConsoleFormatter

    {
        public ConsoleLoggerFormatterConfigureOptions(
            ILoggerProviderConfiguration<AnsiConsoleLoggerProvider> providerConfiguration) : base(
            providerConfiguration.Configuration.GetSection("FormatterOptions"))
        {
        }
    }

    internal class
        ConsoleLoggerFormatterOptionsChangeTokenSource<TFormatter, TOptions> : ConfigurationChangeTokenSource<TOptions>
        where TOptions : AnsiConsoleFormatterOptions
        where TFormatter : class, IConsoleFormatter
    {
        public ConsoleLoggerFormatterOptionsChangeTokenSource(
            ILoggerProviderConfiguration<AnsiConsoleLoggerProvider> providerConfiguration)
            : base(providerConfiguration.Configuration.GetSection("FormatterOptions"))
        {
        }
    }
}