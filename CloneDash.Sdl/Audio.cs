using PathLib;

using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Audio : IAudio
{
    public IntPtr SdlPtr { get; }

    public SDL.AudioSpec AudioSpec { get; }

    public uint Length { get; }

    public static Audio FromWAV(IPath wavFile)
    {
        if (!SDL.LoadWAV(wavFile.ToString()!, out SDL.AudioSpec audioSpec, out IntPtr audioBuffer, out uint audioLength))
            throw new InvalidOperationException(string.Format("Failed to load WAV file {0}: {1}.", wavFile, SDL.GetError()));
        return new(audioBuffer, audioSpec, audioLength);
    }

    internal Audio(IntPtr existing, SDL.AudioSpec audioSpec, uint length) =>
        (SdlPtr, AudioSpec, Length) = (existing, audioSpec, length);

    public void Dispose() => SDL.Free(SdlPtr);
}
