using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CloneDash.Sdk;

/// <summary>An abstract attribute marks the entrypoint of the mod.</summary>
/// <remarks>You need to create your implementation of <see cref="ModEntrypointAttribute" />.
/// We will execute its <see cref="ExtendGame(IServiceCollection, IConfigurationManager, ILogger)" /> method to load your mod.
/// </remarks>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
public abstract class ModEntrypointAttribute : Attribute
{
    /// <summary>Extend the game by injecting services to <paramref name="services" />.</summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to extend.</param>
    /// <param name="configurationManager">The <see cref="IConfigurationManager" /> to load configuration.</param>
    /// <param name="logger">The <see cref="ILogger" /> to generate logging messages.</param>
    public abstract IServiceCollection ExtendGame(IServiceCollection services, IConfigurationManager configurationManager, ILogger logger);
}
