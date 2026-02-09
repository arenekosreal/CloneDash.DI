using SDL3;

namespace CloneDash.Sdl;

internal readonly struct AudioDevice : IAudioDevice
{
    public uint SdlPtr { get; internal init; }

    public float Gain { get => SDL.GetAudioDeviceGain(SdlPtr); set => SDL.SetAudioDeviceGain(SdlPtr, value); }

    public string? Name { get => SDL.GetAudioDeviceName(SdlPtr); }

    public bool Paused
    {
        get => SDL.AudioDevicePaused(SdlPtr);
        set => _ = value ? SDL.PauseAudioDevice(SdlPtr) : SDL.ResumeAudioDevice(SdlPtr);
    }

    public ValueTask<bool> BindStreamsAsync(params IEnumerable<IAudioStream> streams)
    {
        IntPtr[] streamsToSdl = streams.Select(s => s.SdlPtr).ToArray();
        return ValueTask.FromResult(SDL.BindAudioStreams(SdlPtr, streamsToSdl, streamsToSdl.Length));
    }

    public ValueTask DisposeAsync()
    {
        SDL.CloseAudioDevice(SdlPtr);
        return ValueTask.CompletedTask;
    }
}
