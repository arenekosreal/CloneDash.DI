using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloneDash.Configuration;

/// <summary>Extended methods for <see cref="IServiceCollection" />.</summary>
public static class IServiceCollectionExtensions
{
    /// <summary>Add a writable option of type <typeparamref name="T" /> to <paramref name="services" />.</summary>
    public static OptionsBuilder<T> AddWritableOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(this IServiceCollection services, string sectionName)
        where T : class =>
            services.AddTransient<IWritableOptions<T>>(provider =>
                         new WritableOptions<T>(provider.GetRequiredService<IOptionsMonitor<T>>(),
                                                sectionName,
                                                provider.GetRequiredService<IConfigurationWriter>()))
                    .AddOptions<T>(sectionName);

    /// <summary>Add <see cref="IConfigurationWriter" /> to <see cref="IServiceCollection" />.</summary>
    public static IServiceCollection AddConfigurationWriter(this IServiceCollection services, Stream writeTo) =>
        services.AddSingleton<IConfigurationWriter, TomlConfigurationWriter>(provider =>
                     new(writeTo, provider.GetRequiredService<ILogger<TomlConfigurationWriter>>()));
}
