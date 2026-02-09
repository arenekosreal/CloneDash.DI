using PathLib;

namespace CloneDash.Path;

/// <summary>Api requirements for class used for providing paths.</summary>
public interface IPathProvider
{
    /// <summary>Get a writable path for reading/writing configuration file.</summary>
    /// <param name="name">The name of the file.</param>
    /// <returns>The path of the file.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="name" /> contains invalid char.</exception>
    public IPath GetWritableConfigPath(string name);

    /// <summary>Get a readable path for reading configuration file from many possible paths.</summary>
    /// <param name="name">The name of the file.</param>
    /// <returns>The path of the file. <c>null</c> if not found.</returns> 
    /// <exception cref="InvalidOperationException"><paramref name="name" /> contains invalid char.</exception>
    public IPath? GetReadableConfigPath(string name);

    /// <summary>Get a writable path for reading/writing data file.</summary>
    /// <param name="name">The name of the file.</param>
    /// <returns>The path of the file.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="name" /> contains invalid char.</exception>
    public IPath GetWritableDataPath(string name);

    /// <summary>Get a readable path for reading data file from many possible paths.</summary>
    /// <param name="name">The name of the file.</param>
    /// <returns>The path of the file. <c>null</c> if not found.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="name" /> contains invalid char.</exception>
    public IPath? GetReadableDataPath(string name);

    /// <summary>Get a writable path for reading/writing cache file.</summary>
    /// <param name="name">The name of the file.</param>
    /// <returns>The path of the file.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="name" /> contains invalid char.</exception>
    public IPath GetWritableCachePath(string name);

    /// <summary>Get a writable path for reading/writing state file.</summary>
    /// <param name="name">The name of the file.</param>
    /// <returns>The path of the file.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="name" /> contains invalid char.</exception>
    public IPath GetWritableStatePath(string name);

    /// <summary>Enumerate font files in system.</summary>
    public IEnumerable<IPath> EnumerateSystemFonts();
}
