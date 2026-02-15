using SDL3;
using PathLib;

namespace CloneDash.Sdl;

internal readonly struct WAVAudio : IAudio
{
    public IntPtr SdlPtr { get; }

    public SDL.AudioSpec AudioSpec { get; }

    public uint Length { get; }

    public WAVAudio(IPath wavFile)
    {
        if (!SDL.LoadWAV(wavFile.ToString()!, out SDL.AudioSpec audioSpec, out IntPtr audioBuffer, out uint audioLength))
            throw new InvalidOperationException(string.Format("Failed to load WAV file {0}: {1}.", wavFile, SDL.GetError()));
        (SdlPtr, AudioSpec, Length) = (audioBuffer, audioSpec, audioLength);
    }

    public ValueTask DisposeAsync()
    {
        SDL.Free(SdlPtr);
        return ValueTask.CompletedTask;
    }
}
