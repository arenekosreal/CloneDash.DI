namespace CloneDash.Path;

/// <summary>Extended methods for <see cref="string" />.</summary>
public static class StringExtensions
{
    /// <summary>Ensure <paramref name="fileName" /> does not contain any invalid char in file name.</summary>
    /// <exception cref="InvalidOperationException">The <paramref name="fileName" /> contains invalid char.</exception>
    public static void EnsureValidFileName(this string fileName)
    {
        if (string.IsNullOrEmpty(fileName) || fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
            throw new InvalidOperationException($"{fileName} is an invalid file name.");
    }
}
