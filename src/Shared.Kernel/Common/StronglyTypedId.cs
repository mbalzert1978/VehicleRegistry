namespace Shared.Kernel;

/// <summary>
/// Base record for strongly-typed identifiers.
/// </summary>
/// <typeparam name="T">The underlying type of the identifier.</typeparam>
/// <param name="Value">The value of the identifier.</param>
public abstract record StronglyTypedId<T>(T Value)
    where T : notnull;
