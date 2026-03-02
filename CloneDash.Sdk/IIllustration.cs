using CloneDash.Sdl;

namespace CloneDash.Sdk;

/// <summary>An illustration used in game.</summary>
public interface IIllustration : IRenderable
{
    /// <value>The name of the illustration.</value>
    public string Name { get; }

    /// <value>The author of the illustration.</value>
    public string Author { get; }

    /// <value>The possible uri to the illustration.</value>
    public Uri? Uri { get; }

    /// <value>The possible bgm of the illustration.</value>
    public IAudio? Bgm { get; }
}
