using System.Diagnostics.CodeAnalysis;

namespace CloneDash.Configuration;

/// <summary>A writer to write configurations.</summary>
public interface IConfigurationWriter : IAsyncDisposable
{
    /// <summary>Update the configuration section.</summary>
    /// <param name="sectionName">The name of the section.</param>
    /// <param name="sectionValue">The value of the section.</param>
    /// <typeparam name="T">The type of <paramref name="sectionValue" />.</typeparam> 
    /// <remarks>Set to <c>null</c> to remove it from configuration.</remarks>
    public ValueTask UpdateSectionAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(string sectionName, T? sectionValue)
        where T : class;
}
