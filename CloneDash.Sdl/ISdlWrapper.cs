/// <summary>A wrapper around various SDL structs.</summary>
public interface ISdlWrapper<T> : IDisposable
    where T : unmanaged
{
    /// <value>The raw pointer returned by SDL methods.</value>
    public T SdlPtr { get; }
}

