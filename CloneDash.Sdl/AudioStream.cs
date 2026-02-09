using SDL3;

namespace CloneDash.Sdl;

internal readonly struct AudioStream : IAudioStream
{
    public IntPtr SdlPtr { get; internal init; }

    public float Gain { get => SDL.GetAudioStreamGain(SdlPtr); set => SDL.SetAudioStreamGain(SdlPtr, value); }

    public bool Paused
    {
        get => SDL.AudioStreamDevicePaused(SdlPtr);
        set => _ = value ? SDL.PauseAudioStreamDevice(SdlPtr) : SDL.ResumeAudioStreamDevice(SdlPtr);
    }

    public bool Locked { set => _ = value ? SDL.LockAudioStream(SdlPtr) : SDL.UnlockAudioStream(SdlPtr); }

    public ValueTask<bool> ClearAsync() => ValueTask.FromResult(SDL.ClearAudioStream(SdlPtr));

    public ValueTask<bool> FlushAsync() => ValueTask.FromResult(SDL.FlushAudioStream(SdlPtr));

    public ValueTask DisposeAsync()
    {
        SDL.DestroyAudioStream(SdlPtr);
        return ValueTask.CompletedTask;
    }

    public ValueTask UnbindAsync()
    {
        SDL.UnbindAudioStream(SdlPtr);
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> PutAsync(IAudio audio)
    {
        Locked = true;
        bool ret = SDL.PutAudioStreamDataNoCopy(SdlPtr, audio.SdlPtr, Convert.ToInt32(audio.Length), null, IntPtr.Zero);
        Locked = false;
        return ValueTask.FromResult(ret);
    }
}
