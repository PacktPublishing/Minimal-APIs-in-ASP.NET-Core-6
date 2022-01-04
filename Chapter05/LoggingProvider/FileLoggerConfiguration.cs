namespace LoggingProvider
{
    public class FileLoggerConfiguration
    {
        public int EventId { get; set; }
        public string PathFolderName { get; set; } = "logs";
        public bool IsRollingFile { get; set; }
    }
}
