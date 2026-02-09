using Microsoft.Extensions.Logging;

internal class FileLogger : ILogger
{
    private readonly string CategoryName;
    private readonly string DateTimeFormat;
    private readonly string MessageFormat;
    private readonly TextWriter LogFile;
    private readonly LogLevel MinLevel;

    public FileLogger(string categoryName, string dateTimeFormat, string messageFormat, TextWriter logFile, LogLevel minLevel) =>
        (CategoryName, DateTimeFormat, MessageFormat, LogFile, MinLevel) = (categoryName, dateTimeFormat, messageFormat, logFile, minLevel);

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;
        string logText =
            string.Format(MessageFormat,
                          string.Format(DateTimeFormat, DateTime.UtcNow),
                          logLevel, eventId, formatter(state, exception));
        LogFile.WriteLine(logText);
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) => logLevel >= MinLevel;

    /// <inheritdoc />
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    => null;
}
