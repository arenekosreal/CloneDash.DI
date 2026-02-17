using SDL3;

namespace CloneDash.Sdl;

internal readonly struct AudioStream : IAudioStream
{
    public IntPtr SdlPtr { get; }

    public float Gain { get => SDL.GetAudioStreamGain(SdlPtr); set => SDL.SetAudioStreamGain(SdlPtr, value); }

    public bool Paused
    {
        get => SDL.AudioStreamDevicePaused(SdlPtr);
        set => _ = value ? SDL.PauseAudioStreamDevice(SdlPtr) : SDL.ResumeAudioStreamDevice(SdlPtr);
    }

    public int Queued { get => SDL.GetAudioStreamQueued(SdlPtr); }

    public bool Locked { set => _ = value ? SDL.LockAudioStream(SdlPtr) : SDL.UnlockAudioStream(SdlPtr); }

    public AudioStream(IAudioDevice device, SDL.AudioSpec? inputSpec = null)
    {
        if (inputSpec is null)
            SdlPtr = SDL.OpenAudioDeviceStream(device.SdlPtr, IntPtr.Zero, null, IntPtr.Zero);
        else
            SdlPtr = SDL.OpenAudioDeviceStream(device.SdlPtr, inputSpec.Value, null, IntPtr.Zero);
    }

    internal AudioStream(IntPtr existing) => SdlPtr = existing;

    public bool Clear() => SDL.ClearAudioStream(SdlPtr);

    public bool Flush() => SDL.FlushAudioStream(SdlPtr);

    public void Dispose() => SDL.DestroyAudioStream(SdlPtr);

    public void Unbind() => SDL.UnbindAudioStream(SdlPtr);

    public bool Put(IAudio audio) =>
        SDL.PutAudioStreamDataNoCopy(SdlPtr, audio.SdlPtr,
                                     Convert.ToInt32(audio.Length),
                                     null, IntPtr.Zero);
}
