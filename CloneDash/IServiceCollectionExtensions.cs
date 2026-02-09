using Microsoft.Extensions.DependencyInjection;

namespace CloneDash;

internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMainScene(this IServiceCollection services) =>
        services.AddSingleton<IMainScene, MainScene>();
}
