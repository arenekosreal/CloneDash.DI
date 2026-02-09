using PathLib;

namespace CloneDash.Path;

/// <summary><see cref="IPathProvider" /> implementaion for Linux/FreeBSD, etc.</summary>
public class XdgPathProvider : IPathProvider
{
    private readonly string AppId;

    private readonly IPath Home = new PosixPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

    private IPath XdgConfigHome
    {
        get
        {
            string? xdgConfigHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
            return string.IsNullOrEmpty(xdgConfigHome) ? Home / ".config" : new PosixPath(xdgConfigHome);
        }
    }

    private IPath XdgCacheHome
    {
        get
        {
            string? xdgCacheHome = Environment.GetEnvironmentVariable("XDG_CACHE_HOME");
            return string.IsNullOrEmpty(xdgCacheHome) ? Home / ".cache" : new PosixPath(xdgCacheHome);
        }
    }

    private readonly IPath XdgDataHome = new PosixPath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

    private IPath XdgStateHome
    {
        get
        {
            string? xdgStateHome = Environment.GetEnvironmentVariable("XDG_STATE_HOME");
            return string.IsNullOrEmpty(xdgStateHome) ? Home / ".local" / "state" : new PosixPath(xdgStateHome);
        }
    }

    private IEnumerable<IPath> XdgConfigDirs
    {
        get
        {
            string? xdgConfigDirs = Environment.GetEnvironmentVariable("XDG_CONFIG_DIRS");
            return (string.IsNullOrEmpty(xdgConfigDirs) ? "/etc/xdg" : xdgConfigDirs).Split(':').Select((p) => new PosixPath(p));
        }
    }

    private IEnumerable<IPath> XdgDataDirs
    {
        get
        {
            string? xdgDataDirs = Environment.GetEnvironmentVariable("XDG_DATA_DIRS");
            return (string.IsNullOrEmpty(xdgDataDirs) ? "/usr/local/share:/usr/share" : xdgDataDirs).Split(':').Select((p) => new PosixPath(p));
        }
    }

    /// <summary>Initialize <see cref="XdgPathProvider" /> with arguments given.</summary>
    /// <param name="appId">The id of application.</param>
    public XdgPathProvider(string appId) => AppId = appId;

    /// <inheritdoc />
    public IPath GetWritableConfigPath(string name) => XdgConfigHome / AppId / name;

    /// <inheritdoc />
    public IPath? GetReadableConfigPath(string name)
    {
        foreach (IPath configHome in XdgConfigDirs.Prepend(XdgConfigHome))
        {
            IPath configPath = configHome / AppId / name;
            if (configPath.Exists())
                return configPath;
        }
        return null;
    }

    /// <inheritdoc />
    public IPath GetWritableDataPath(string name) => XdgDataHome / AppId / name;

    /// <inheritdoc />
    public IPath? GetReadableDataPath(string name)
    {
        foreach (IPath dataHome in XdgDataDirs.Prepend(XdgDataHome))
        {
            IPath dataPath = dataHome / AppId / name;
            if (dataPath.Exists())
                return dataPath;
        }
        return null;
    }

    /// <inheritdoc />
    public IPath GetWritableCachePath(string name) => XdgCacheHome / AppId / name;

    /// <inheritdoc />
    public IPath GetWritableStatePath(string name) => XdgStateHome / AppId / name;

    /// <inheritdoc />
    public IPath? GetSystemFontPath(string fontFamilyName, string? variant = null)
    {
        IPath[] possiblePaths = new[]
        {
            // $HOME/.fonts
            new PosixPath(Environment.GetFolderPath(Environment.SpecialFolder.Fonts)),
        }.Append(XdgDataHome / "fonts").Concat(XdgDataDirs.Select(d => d / "fonts")).ToArray();
        string fontFullName = fontFamilyName + variant is not null ? $"-{variant}" : string.Empty;
        string? fontFullPath = possiblePaths.SelectMany(d => d.DirectoryInfo.EnumerateFiles(fontFullName, SearchOption.AllDirectories))
                                            .FirstOrDefault(f => f.Exists)?.FullName;
        return fontFullPath is not null ? new PosixPath(fontFullPath) : null;
    }

    /// <inheritdoc />
    public IEnumerable<IPath> EnumerateSystemFonts()
    {
        IPath[] possibleFontsDirs = new[]
        {
            // $HOME/.fonts
            new PosixPath(Environment.GetFolderPath(Environment.SpecialFolder.Fonts)),
            XdgDataHome / "fonts"
        }.Concat(XdgDataDirs.Select(d => d / "fonts")).ToArray();
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
