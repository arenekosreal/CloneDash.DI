using System.Runtime.Versioning;

using PathLib;

namespace CloneDash.Path;

/// <summary><see cref="IPathProvider" /> implementation for Windows.</summary>
[SupportedOSPlatform("Windows")]
public class WindowsPathProvider : IPathProvider
{
    private readonly string AppId;
    // %USERPROFILE%\AppData\Roaming
    private readonly IPath ApplicationData = new WindowsPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

    // %USERPROFILE%\AppData\Local
    private readonly IPath LocalApplicationData = new WindowsPath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

    private IPath Temp
    {
        get
        {
            string? temp = Environment.GetEnvironmentVariable("TMP") ?? Environment.GetEnvironmentVariable("TEMP");
            return string.IsNullOrEmpty(temp) ? LocalApplicationData / "Temp" : new WindowsPath(temp);
        }
    }

    /// <summary>Initialize <see cref="WindowsPathProvider" /> with arguments given.</summary>
    /// <param name="appId">The id of application.</param>
    public WindowsPathProvider(string appId) => AppId = appId;

    /// <inheritdoc />
    public IPath GetWritableConfigPath(string name) => ApplicationData / AppId / name;

    /// <inheritdoc />
    public IPath? GetReadableConfigPath(string name)
    {
        IPath configPath = ApplicationData / AppId / name;
        return configPath.Exists() ? configPath : null;
    }

    /// <inheritdoc />
    public IPath GetWritableDataPath(string name) => LocalApplicationData / AppId / name;

    /// <inheritdoc />
    public IPath? GetReadableDataPath(string name)
    {
        IPath dataPath = LocalApplicationData / AppId / name;
        return dataPath.Exists() ? dataPath : null;
    }

    /// <inheritdoc />
    public IPath GetWritableCachePath(string name) => Temp / AppId / name;

    /// <inheritdoc />
    public IPath GetWritableStatePath(string name) => Temp / AppId / name;

    /// <inheritdoc />
    public IEnumerable<IPath> EnumerateSystemFonts()
    {
        IPath[] possibleFontsDirs = new[]
        {
            // %SYSTEMROOT%\Fonts
            new WindowsPath(Environment.GetFolderPath(Environment.SpecialFolder.Fonts)),
            ApplicationData / "Microsoft" / "Windows" / "Fonts",
            LocalApplicationData / "Microsoft" / "Windows" / "Fonts,"
        };
        foreach (IPath fontDir in possibleFontsDirs)
        {
            if (fontDir.IsDir())
            {
                foreach (IPath font in fontDir.ListDir("*.ttf", SearchOption.AllDirectories))
                {
                    yield return font;
                }
            }
        }
    }
}
