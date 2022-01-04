namespace LoggingProvider
{
    public class FileLogger : ILogger
    {
        private readonly string name;
        private readonly Func<FileLoggerConfiguration> getCurrentConfig;

        public FileLogger(string name, Func<FileLoggerConfiguration> getCurrentConfig)
        {
            this.name = name;
            this.getCurrentConfig = getCurrentConfig;
        }

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var config = getCurrentConfig();
            if (config.EventId == 0 || config.EventId == eventId.Id)
            {
                string line = $"{name} - {formatter(state, exception)}";
                string fileName = config.IsRollingFile ? RollingFileName : FullFileName;
                string fullPath = Path.Combine(config.PathFolderName, fileName);
                File.AppendAllLines(fullPath, new[] { line });
            }
        }

        private static string RollingFileName => $"log-{DateTime.UtcNow:yyyy-MM-dd}.txt";
        private const string FullFileName = "logs.txt";
    }
}
