using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.ValueObjects;

/// <summary>
/// Represents a hashed password value object.
/// Never stores plain-text passwords - only hashed values.
/// </summary>
public sealed record PasswordHash(string Value);

/// <summary>
/// Factory for creating PasswordHash instances with validation.
/// </summary>
public static class PasswordHashFactory
{
    /// <summary>
    /// Creates a PasswordHash from a hashed password string.
    /// </summary>
    /// <param name="hash">The hashed password (never plain-text).</param>
    /// <returns>Result containing the PasswordHash or an error.</returns>
    public static Result<PasswordHash> Create(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            Error error = ErrorFactory.Validation(
                "PasswordHash",
                "Password hash cannot be empty or whitespace."
            );

            Debug.Assert(error.Code == "PASSWORDHASH.Validation", "Error code should match");

            return ResultFactory.Failure<PasswordHash>(error);
        }

        PasswordHash passwordHash = new(hash);

        Debug.Assert(passwordHash.Value == hash, "PasswordHash value should match input");
        Debug.Assert(
            !string.IsNullOrWhiteSpace(passwordHash.Value),
            "PasswordHash value should not be empty"
        );

        return ResultFactory.Success(passwordHash);
    }
}
