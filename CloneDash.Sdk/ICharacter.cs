using CloneDash.Sdl;

namespace CloneDash.Sdk;

/// <summary>A character used in game to play levels.</summary>
public interface ICharacter : IRenderable
{
    /// <value>The name of the character.</value>
    public string Name { get; }

    /// <value>The description of the character.</value>
    public string Description { get; }

    /// <summary>Apply character's skill.</summary>
    public void ApplySkill();
}
