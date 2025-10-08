namespace Shared.Kernel;

/// <summary>
/// Represents the result of an operation using Railway-Oriented Programming pattern.
/// This is a discriminated union with two variants: Success and Failure.
/// </summary>
public abstract record Result
{
    /// <summary>
    /// Gets a value indicating whether the result represents a successful operation.
    /// </summary>
    public abstract bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result represents a failed operation.
    /// </summary>
    public abstract bool IsFailure { get; }
}

/// <summary>
/// Represents a successful operation result.
/// </summary>
public sealed record Success : Result
{
    /// <inheritdoc />
    public override bool IsSuccess => true;

    /// <inheritdoc />
    public override bool IsFailure => !IsSuccess;
}

/// <summary>
/// Represents a failed operation result with an associated error.
/// </summary>
/// <param name="Error">The error that caused the failure.</param>
public sealed record Failure(Error Error) : Result
{
    /// <inheritdoc />
    public override bool IsSuccess => false;

    /// <inheritdoc />
    public override bool IsFailure => !IsSuccess;
}

/// <summary>
/// Factory for creating Result instances.
/// </summary>
public static class ResultFactory
{
    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>A Success result instance.</returns>
    public static Result Success() => new Success();

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    /// <returns>A Failure result instance containing the error.</returns>
    public static Result Failure(Error error) => new Failure(error);
}
