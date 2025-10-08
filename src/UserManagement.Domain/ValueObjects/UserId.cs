using System.Diagnostics;
using UserManagement.Domain.Common;

namespace UserManagement.Domain.ValueObjects;

/// <summary>
/// Strongly-typed identifier for User entities.
/// </summary>
/// <param name="Value">The unique identifier value.</param>
public sealed record UserId(Guid Value) : StronglyTypedId<Guid>(Value);

/// <summary>
/// Factory for creating UserId instances.
/// </summary>
public static class UserIdFactory
{
    /// <summary>
    /// Creates a new UserId with a version 7 GUID.
    /// </summary>
    /// <returns>A new <see cref="UserId"/> instance with a unique version 7 GUID.</returns>
    public static UserId Create()
    {
        var value = Guid.CreateVersion7();
        Debug.Assert(value != Guid.Empty, "Generated GUID cannot be empty.");

        UserId result = new(value);
        Debug.Assert(result.Value == value, "UserId value must match the provided GUID.");

        return result;
    }

    /// <summary>
    /// Creates a UserId from an existing GUID value.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    /// <returns>A new <see cref="UserId"/> instance with the specified value.</returns>
    public static UserId Create(Guid value)
    {
        Debug.Assert(value != Guid.Empty, "UserId value cannot be the default GUID.");

        UserId result = new(value);

        Debug.Assert(result.Value == value, "UserId value must match the provided GUID.");
        Debug.Assert(result.Value != Guid.Empty, "UserId value cannot be empty after creation.");

        return result;
    }
}
