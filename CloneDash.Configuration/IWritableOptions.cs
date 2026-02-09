using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

namespace CloneDash.Configuration;

/// <summary>A writable <see cref="IOptions{TOptions}" />.</summary>
public interface IWritableOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T> : IOptions<T>, IAsyncDisposable
    where T : class
{ }
