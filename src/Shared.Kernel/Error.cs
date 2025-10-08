namespace Shared.Kernel;

/// <summary>
/// Represents an error with a code and message.
/// </summary>
/// <param name="Code">The error code.</param>
/// <param name="Message">The error message.</param>
public sealed record Error(string Code, string Message);

/// <summary>
/// Factory for creating Error instances.
/// </summary>
public static class ErrorFactory
{
    /// <summary>
    /// Represents an empty error (no error occurred).
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);
}
