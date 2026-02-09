using CloneDash.Path;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using PathLib;

namespace CloneDash.Logging;

[ProviderAlias("File")]
internal class FileLoggerProvider : ILoggerProvider
{
    private readonly IOptions<Dictionary<string, LogLevel>> LogLevelMap;
    private readonly IOptions<FileLoggerConfiguration> LoggerConfigurationSnapshot;
    private readonly IPath LogsDirectory;
    private readonly Dictionary<string, ILogger> Loggers = new();
    private readonly Dictionary<IPath, FileStream> Streams = new();
    private readonly Dictionary<IPath, TextWriter> TextWriters = new();

    public FileLoggerProvider(IOptions<Dictionary<string, LogLevel>> logLevelMap,
                              IOptions<FileLoggerConfiguration> loggerConfigurationSnapshot,
                              IPathProvider pathProvider)
    {
        (LogLevelMap, LoggerConfigurationSnapshot) = (logLevelMap, loggerConfigurationSnapshot);
        LogsDirectory = pathProvider.GetWritableStatePath("logs");
    }

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        if (!Loggers.ContainsKey(categoryName))
        {
            FileLoggerConfiguration configuration = LoggerConfigurationSnapshot.Value;
            IPath logFilePath = LogsDirectory / string.Format(configuration.FileNameFormat, categoryName, DateTime.UtcNow);
            if (LogsDirectory.Exists() && !LogsDirectory.IsDir())
                LogsDirectory.Delete();
            if (!LogsDirectory.Exists())
                LogsDirectory.Mkdir(makeParents: true);
            if (!TextWriters.ContainsKey(logFilePath))
            {
                if (!Streams.ContainsKey(logFilePath))
                {
                    Streams[logFilePath] = logFilePath.Open(FileMode.OpenOrCreate);
                }
                TextWriters[logFilePath] = new StreamWriter(Streams[logFilePath]);
            }
            LogLevel minLevel = LogLevelMap.Value.TryGetValue(categoryName, out minLevel) ? minLevel : LogLevelMap.Value["Default"];
            Loggers[categoryName] = new FileLogger(categoryName,
                                                   configuration.DateTimeFormat,
                                                   configuration.MessageFormat,
                                                   TextWriters[logFilePath],
                                                   minLevel);
        }
        return Loggers[categoryName];
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Loggers.Clear();
        foreach (TextWriter textWriter in TextWriters.Values)
            textWriter.Dispose();
        TextWriters.Clear();
        foreach (FileStream stream in Streams.Values)
            stream.Dispose();
        Streams.Clear();
    }
}
