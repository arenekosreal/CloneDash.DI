using CsToml;

namespace CloneDash.Logging;

/// <summary>The configuration of <c>Logging:File</c> section.</summary>
[TomlSerializedObject]
public partial class FileLoggerConfiguration
{
    /// <value>The file name of log.</value>
    [TomlValueOnSerialized]
    public string FileNameFormat { get; set; } = "application.log";

    /// <value>The format string of message.</value>
    /// <remarks>Elements order:
    /// &lt;datetime&gt;,
    /// &lt;loglevel&gt;,
    /// &lt;eventid&gt;,
    /// &lt;log message&gt;.
    /// </remarks>
    [TomlValueOnSerialized]
    public string MessageFormat { get; set; } = string.Empty;

    /// <value>The format string of datetime.</value>
    /// <remarks>Elements:
    /// &lt;UTC timestamp&gt;.
    /// </remarks> 
    [TomlValueOnSerialized]
    public string DateTimeFormat { get; set; } = string.Empty;
}
