using System.Diagnostics.CodeAnalysis;
using System.Text;

using CsToml;

using Microsoft.Extensions.Logging;

namespace CloneDash.Configuration;

internal class TomlConfigurationWriter : IConfigurationWriter
{
    private readonly CsTomlSerializerOptions SerializerOptions = CsTomlSerializerOptions.Default with
    {
        SerializeOptions = new()
        {
            TableStyle = TomlTableStyle.Header,
            ArrayStyle = TomlArrayStyle.Header,
            DefaultNullHandling = TomlNullHandling.Ignore,
        },
    };
    private readonly Dictionary<string, Stream> Sections = new();
    private readonly Stream Target;
    private readonly ILogger<TomlConfigurationWriter> Logger;

    public TomlConfigurationWriter(Stream target, ILogger<TomlConfigurationWriter> logger) =>
        (Target, Logger) = (target, logger);

    public async ValueTask UpdateSectionAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(string sectionName, T? sectionValue)
        where T : class
    {
        if (Sections.ContainsKey(sectionName))
        {
            if (sectionValue is not null)
            {
                Sections[sectionName].Seek(0, SeekOrigin.Begin);
                Logger.LogDebug("Found existing section {0}.", sectionName);
            }
            else
            {
                Sections.Remove(sectionName);
                Logger.LogDebug("Removed section {0}.", sectionName);
                return;
            }
        }
        else
        {
            if (sectionValue is not null)
            {
                Sections[sectionName] = new MemoryStream();
                Logger.LogDebug("Created new section {0}...", sectionName);
            }
            else
            {
                return;
            }
        }
        await CsTomlSerializer.SerializeAsync(Sections[sectionName],
                                              sectionValue,
                                              SerializerOptions);
        await Sections[sectionName].FlushAsync();
        Logger.LogDebug("Written {0} byte(s) into buffer stream.", Sections[sectionName].Length);
    }

    public async ValueTask DisposeAsync()
    {
        Target.Seek(0, SeekOrigin.Begin);
        foreach ((string sectionName, Stream sectionStream) in Sections)
        {
            sectionStream.Seek(0, SeekOrigin.Begin);
            await Target.WriteAsync(Encoding.UTF8.GetBytes($"[{sectionName}]\n"));
            await sectionStream.CopyToAsync(Target, (int)sectionStream.Length);
            await sectionStream.DisposeAsync();
        }
        await Target.FlushAsync();
        Logger.LogDebug("Written {0} byte(s) to target stream.", Target.Length);
    }
}
