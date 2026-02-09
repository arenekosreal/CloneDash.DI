using SDL3;

namespace CloneDash.Sdl;

/// <summary>Wraps an allocated buffer by using functions like <see cref="SDL.LoadWAV(string, out SDL.AudioSpec, out nint, out uint)" />.</summary>
public interface IAudio : ISdlWrapper<IntPtr>
{
    /// <value>The <see cref="SDL.AudioSpec" /> returned by load function.</value>
    public SDL.AudioSpec AudioSpec { get; }

    /// <value>The buffer length returned by load function.</value>
    public uint Length { get; }
}
