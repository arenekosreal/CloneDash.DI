using CsToml;

using SDL3;

namespace CloneDash.Sdl;

[TomlSerializedObject(NamingConvention = TomlNamingConvention.None)]
internal partial class SdlConfiguration
{
    [TomlValueOnSerialized]
    public int Height { get; set; } = 768;

    [TomlValueOnSerialized]
    public int Width { get; set; } = 1024;

    [TomlValueOnSerialized]
    public SDL.WindowFlags SdlFlags { get; set; } = SDL.WindowFlags.Vulkan;
}
