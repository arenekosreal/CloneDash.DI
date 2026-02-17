namespace CloneDash.Sdl;

/// <summary>Wrapper of <see href="https://wiki.libsdl.org/SDL3/SDL_AudioStream" />.</summary>
public interface IAudioStream : ISdlWrapper<IntPtr>
{
    /// <value>The volume of the stream.</value>
    public float Gain { get; set; }

    /// <value>If the stream's device is paused.</value>
    public bool Paused { get; set; }

    /// <value>How many bytes are queued in the stream.</value>
    public int Queued { get; }

    /// <value>If the stream is locked.</value>
    public bool Locked { set; }

    /// <summary>Clear the stream.</summary>
    public bool Clear();

    /// <summary>Flush the stream.</summary>
    public bool Flush();

    /// <summary>Unbind the devices of the stream.</summary>
    public void Unbind();

    /// <summary>Put audio data to the stream.</summary>
    public bool Put(IAudio audio);
}
