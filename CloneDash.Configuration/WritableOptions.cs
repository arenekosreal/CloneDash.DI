using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

namespace CloneDash.Configuration;

internal class WritableOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T> : IWritableOptions<T>
    where T : class
{

    private readonly IOptionsMonitor<T> OptionsMonitor;
    private readonly IConfigurationWriter ConfigurationWriter;
    private readonly string SectionName;

    /// <summary>Initialize <see cref="WritableOptions{T}" /> with arguments given.</summary>
    public WritableOptions(IOptionsMonitor<T> optionsMonitor, string sectionName, IConfigurationWriter configurationWriter) =>
        (OptionsMonitor, SectionName, ConfigurationWriter) = (optionsMonitor, sectionName, configurationWriter);

    /// <inheritdoc />
    public T Value { get => OptionsMonitor.CurrentValue; }

    /// <inheritdoc />
    public async ValueTask DisposeAsync() =>
        await ConfigurationWriter.UpdateSectionAsync(SectionName, Value);
}
