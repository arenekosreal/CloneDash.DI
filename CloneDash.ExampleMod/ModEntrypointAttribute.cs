using CloneDash.Sdk;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CloneDash.ExampleMod;

/// <summary><see cref="ModEntrypointAttribute" /> implementation for ExampleMod.</summary>
public class ModEntrypointAttribute : CloneDash.Sdk.ModEntrypointAttribute
{
    /// <inheritdoc />
    public override IServiceCollection ExtendGame(IServiceCollection services, IConfigurationManager configurationManager, ILogger logger)
    {
        return services;
    }
}
