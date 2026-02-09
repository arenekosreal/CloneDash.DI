namespace CloneDash.Sdl;

/// <summary>Wrapper of <see href="https://wiki.libsdl.org/SDL3/SDL_AudioDeviceID" />.</summary>
public interface IAudioDevice : ISdlWrapper<uint>
{
    /// <value>The volume of the device.</value>
    public float Gain { get; set; }

    /// <value>The name of the device.</value>
    public string? Name { get; }

    /// <value>If the device is paused.</value>
    public bool Paused { get; set; }

    /// <summary>Bind a list of audio streams to an audio device.</summary>
    /// <returns><c>true</c> on success or <c>false</c>on failure.</returns> 
    public ValueTask<bool> BindStreamsAsync(params IEnumerable<IAudioStream> streams);
}
