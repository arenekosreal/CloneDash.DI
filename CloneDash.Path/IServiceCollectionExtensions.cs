using Microsoft.Extensions.DependencyInjection;

namespace CloneDash.Path;

/// <summary>Extended methods for <see cref="IServiceCollection" />.</summary>
public static class IServiceCollectionExtensions
{
    /// <summary>Add <see cref="IPathProvider" /> to <see cref="IServiceCollection" />.</summary>
    public static IServiceCollection AddPathProvider(this IServiceCollection services, IPathProvider pathProvider) => services.AddSingleton<IPathProvider>(pathProvider);
}
