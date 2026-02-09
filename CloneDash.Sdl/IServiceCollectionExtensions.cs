using CloneDash.Configuration;
using CloneDash.Path;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CloneDash.Sdl;

/// <summary>Extended methods for <see cref="IServiceCollection" />.</summary>
public static class IServiceCollectionExtensions
{
    /// <summary>Add <see cref="ISdl" /> to <see cref="IServiceCollection" />.</summary>
    public static IServiceCollection AddSdl(this IServiceCollection services, IConfigurationManager configurationManager, string appName, string appVersion, string appId)
    {
        services.AddSingleton<ISdl, Sdl>(provider =>
                    new(provider.GetRequiredService<ILogger<Sdl>>(),
                            provider.GetRequiredService<IWritableOptions<SdlConfiguration>>(),
                            provider.GetRequiredService<IPathProvider>(),
                            appName, appVersion, appId))
                .AddWritableOptions<SdlConfiguration>(nameof(Sdl))
                .Configure(configurationManager.GetSection(nameof(Sdl)).Bind);
        return services;
    }
}
