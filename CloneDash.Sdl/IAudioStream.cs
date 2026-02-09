namespace CloneDash.Sdl;

/// <summary>Wrapper of <see href="https://wiki.libsdl.org/SDL3/SDL_AudioStream" />.</summary>
public interface IAudioStream : ISdlWrapper<IntPtr>
{
    /// <value>The volume of the stream.</value>
    public float Gain { get; set; }

    /// <value>If the stream's device is paused.</value>
    public bool Paused { get; set; }

    /// <value>If the stream is locked.</value>
    public bool Locked { set; }

    /// <summary>Clear the stream.</summary>
    public ValueTask<bool> ClearAsync();

    /// <summary>Flush the stream.</summary>
    public ValueTask<bool> FlushAsync();

    /// <summary>Unbind the devices of the stream.</summary>
    public ValueTask UnbindAsync();

    /// <summary>Put audio data to the stream.</summary>
    public ValueTask<bool> PutAsync(IAudio audio);
}
