using SDL3;

namespace CloneDash.Sdl;

internal readonly struct Audio : IAudio
{
    public IntPtr SdlPtr { get; internal init; }

    public SDL.AudioSpec AudioSpec { get; internal init; }

    public uint Length { get; internal init; }

    public ValueTask DisposeAsync()
    {
        SDL.Free(SdlPtr);
        return ValueTask.CompletedTask;
    }
}
