using Microsoft.Extensions.DependencyInjection;

namespace CloneDash;

internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMainScene(this IServiceCollection services) =>
        services.AddTransient<IMainScene, MainScene>();

    public static IServiceCollection AddKanban(this IServiceCollection services) =>
        services.AddTransient<IKanban, Kanban>();

}
