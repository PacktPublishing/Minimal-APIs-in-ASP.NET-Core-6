namespace LoggingSourceGenerator
{
    public partial class LogGenerator
    {
        private readonly ILogger<LogGeneratorCategory> _logger;

        public LogGenerator(ILogger<LogGeneratorCategory> logger)
        {
            _logger = logger;
        }

        [LoggerMessage(
            EventId = 23,
            Level = LogLevel.Critical,
            Message = "Database error: `{sqlError}`")]
        public partial void DatabaseError(string sqlError);

        [LoggerMessage(
            EventId = 100,
            EventName = "Start",
            Level = LogLevel.Debug,
            Message = "Start Endpoint: {endpointName} with data {dataIn}")]
        public partial void StartEndpointSignal(string endpointName, object dataIn);

        [LoggerMessage(
           EventId = 101,
           EventName = "StartFiltered",
           Message = "Log level filtered: {endpointName} with data {dataIn}")]
        public partial void LogLevelFilteredAtRuntime(LogLevel logLevel, string endpointName, object dataIn);
    }

    public class LogGeneratorCategory { }
}
