using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;

namespace LoggingProvider
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFile(
        this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions<FileLoggerConfiguration, FileLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddFile(
            this ILoggingBuilder builder,
            Action<FileLoggerConfiguration> configure)
        {
            builder.AddFile();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
