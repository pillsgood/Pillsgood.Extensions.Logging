using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Pillsgood.Extensions.Logging
{
    public static class ConsoleLoggerExtensions
    {
        public static ILoggingBuilder AddAnsi24BitConsole(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.AddAnsi24BitConsoleFormatter<DefaultConsoleFormatter, DefaultConsoleFormatterOptions>();
            //TODO : more console formatters

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, Ansi24BitConsoleLoggerProvider>());
            LoggerProviderOptions
                .RegisterProviderOptions<ConsoleLoggerOptions, Ansi24BitConsoleLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddAnsi24BitConsole(this ILoggingBuilder builder, Action<ConsoleLoggerOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddAnsi24BitConsole();
            builder.Services.Configure(configure);
            return builder;
        }

        public static ILoggingBuilder AddDefaultAnsi24BitConsole(this ILoggingBuilder builder,
            Action<DefaultConsoleFormatterOptions> configure) =>
            builder.AddAnsi24BitConsoleWithFormatter(ConsoleFormatterNames.Default, configure);

        internal static ILoggingBuilder AddAnsi24BitConsoleWithFormatter<TOptions>(this ILoggingBuilder builder, string name,
            Action<TOptions> configure) where TOptions : ConsoleFormatterOptions
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
            builder.AddAnsi24BitConsole(options => options.FormatterName = name);

        public static ILoggingBuilder AddAnsi24BitConsoleFormatter<TFormatter, TOptions>(this ILoggingBuilder builder)
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

        public static ILoggingBuilder AddAnsi24BitConsoleFormatter<TFormatter, TOptions>(this ILoggingBuilder builder,
            Action<TOptions> configure)
            where TOptions : ConsoleFormatterOptions
            where TFormatter : class, IConsoleFormatter
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddAnsi24BitConsoleFormatter<TFormatter, TOptions>();
            builder.Services.Configure(configure);
            return builder;
        }
    }

    internal class
        ConsoleLoggerFormatterConfigureOptions<TFormatter, TOptions> : ConfigureFromConfigurationOptions<TOptions>
        where TOptions : ConsoleFormatterOptions
        where TFormatter : class, IConsoleFormatter

    {
        public ConsoleLoggerFormatterConfigureOptions(
            ILoggerProviderConfiguration<Ansi24BitConsoleLoggerProvider> providerConfiguration) : base(
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
            ILoggerProviderConfiguration<Ansi24BitConsoleLoggerProvider> providerConfiguration)
            : base(providerConfiguration.Configuration.GetSection("FormatterOptions"))
        {
        }
    }
}