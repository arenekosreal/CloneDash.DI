using System.Globalization;

using CloneDash.Configuration;
using CloneDash.Path;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SDL3;

namespace CloneDash.Sdl;

/// <summary>Extended methods for <see cref="IServiceCollection" />.</summary>
public static class IServiceCollectionExtensions
{
    private const int FontSize = 24;
    private static IServiceCollection AddWellKnownFonts(this IServiceCollection services)
    {
        services.AddKeyedTransient<IFont>(WellKnownFonts.Monospace, (provider, _) =>
        {
            IFont? font = provider.GetRequiredService<ISdl>().GetFont("Noto Sans Mono", FontSize,
                TTF.FontStyleFlags.Normal, TTF.HintingFlags.Normal);
            if (font is null)
                throw new NullReferenceException("Noto Sans Mono font is not found.");
            return font.Copy();
        });
        string notoCjkVariant = CultureInfo.CurrentUICulture.LCID switch
        {
            // See also:
            // https://learn.microsoft.com/zh-cn/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c
            0x0004 => "SC", // zh-Hans >= Windows NT 3.51
            0x7804 => "SC", // zh      >= Windows 7
            0x0804 => "SC", // zh-CN   >= Windows NT 3.51
            0x1004 => "SC", // zh-SG   >= Windows NT 3.51
            0x7c04 => "TC", // zh-Hant >= Windows NT 3.51
            0x0c04 => "HK", // zh-HK   >= Windows NT 3.51
            0x1404 => "TC", // zh-MO   >= Windows XP
            0x0404 => "TC", // zh-TW   >= Windows NT 3.51
            0x0011 => "JP", // ja      >= Windows 7
            0x0411 => "JP", // ja-JP   >= Windows NT 3.51
            0x0012 => "KR", // ko      >= Windows 7
            0x0412 => "KR", // ko-KR   >= Windows NT 3.51
            0x1000 => "KR", // ko-KP   >= Windows 10 1607
            _ => "SC"
        };
        string notoSansCJK = $"Noto Sans CJK {notoCjkVariant}";
        services.AddKeyedTransient<IFont>(WellKnownFonts.SansSerif, (provider, _) =>
        {
            IFont? font = provider.GetRequiredService<ISdl>().GetFont(notoSansCJK, FontSize,
                TTF.FontStyleFlags.Normal, TTF.HintingFlags.Normal);
            if (font is null)
                throw new NullReferenceException(notoSansCJK + " font is not found.");
            return font.Copy();
        });
        return services;
    }

    /// <summary>Add <see cref="ISdl" /> to <see cref="IServiceCollection" />.</summary>
    public static IServiceCollection AddSdl(this IServiceCollection services, IConfigurationManager configurationManager, string appName, string appVersion, string appId)
    {
        services.AddSingleton<ISdl, Sdl>(provider =>
                    new(provider.GetRequiredService<ILogger<Sdl>>(),
                            provider.GetRequiredService<IWritableOptions<SdlConfiguration>>(),
                            provider.GetRequiredService<IPathProvider>(),
                            appName, appVersion, appId))
                .AddWellKnownFonts()
                .AddWritableOptions<SdlConfiguration>(nameof(Sdl))
                .Configure(configurationManager.GetSection(nameof(Sdl)).Bind);
        return services;
    }
}
