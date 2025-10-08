namespace Shared.Kernel;

/// <summary>
/// Represents the result of an operation that returns a value, using Railway-Oriented Programming pattern.
/// This is a discriminated union with two variants: Success&lt;T&gt; and Failure&lt;T&gt;.
/// </summary>
/// <typeparam name="T">The type of the value returned on success.</typeparam>
public abstract record Result<T>
{
    /// <summary>
    /// Gets a value indicating whether the result represents a successful operation.
    /// </summary>
    public abstract bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result represents a failed operation.
    /// </summary>
    public abstract bool IsFailure { get; }

    /// <summary>
    /// Gets the value of a successful result.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when accessing Value on a Failure result.</exception>
    public abstract T Value { get; }
}

/// <summary>
/// Represents a successful operation result with a value.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
/// <param name="Value">The value of the successful operation.</param>
public sealed record Success<T>(T Value) : Result<T>
{
    /// <inheritdoc />
    public override bool IsSuccess => true;

    /// <inheritdoc />
    public override bool IsFailure => !IsSuccess;

    /// <inheritdoc />
    public override T Value { get; } = Value;
}

/// <summary>
/// Represents a failed operation result with an associated error.
/// </summary>
/// <typeparam name="T">The type of the value that would have been returned on success.</typeparam>
/// <param name="Error">The error that caused the failure.</param>
public sealed record Failure<T>(Error Error) : Result<T>
{
    private const string UnwrapError = "Cannot access Value on a Failure result.";

    /// <inheritdoc />
    public override bool IsSuccess => false;

    /// <inheritdoc />
    public override bool IsFailure => !IsSuccess;

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">Always thrown when accessing Value on a Failure result.</exception>
    public override T Value => throw new InvalidOperationException(UnwrapError);
}
