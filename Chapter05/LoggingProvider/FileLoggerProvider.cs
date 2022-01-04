using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace LoggingProvider
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable onChangeToken;
        private FileLoggerConfiguration currentConfig;
        private readonly ConcurrentDictionary<string, FileLogger> _loggers = new();

        public FileLoggerProvider(IOptionsMonitor<FileLoggerConfiguration> config)
        {
            currentConfig = config.CurrentValue;
            CheckDirectory();
            onChangeToken = config.OnChange(updateConfig =>
            {
                currentConfig = updateConfig;
                CheckDirectory();
            });
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new FileLogger(name, () => currentConfig));
        }

        public void Dispose()
        {
            _loggers.Clear();
            onChangeToken.Dispose();
        }

        private void CheckDirectory()
        {
            if (!Directory.Exists(currentConfig.PathFolderName))
                Directory.CreateDirectory(currentConfig.PathFolderName);
        }
    }
}
