using UserManagement.Domain.Common;

namespace UserManagement.Domain.ValueObjects;

/// <summary>
/// Strongly-typed identifier for User entities.
/// </summary>
/// <param name="Value">The unique identifier value.</param>
public sealed record UserId(Guid Value) : StronglyTypedId<Guid>(Value);
