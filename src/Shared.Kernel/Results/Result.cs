using System.Diagnostics;

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
    public static Result Success()
    {
        Result result = new Success();

        Debug.Assert(result.IsSuccess, "Success result must have IsSuccess == true.");
        Debug.Assert(!result.IsFailure, "Success result must have IsFailure == false.");

        return result;
    }

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    /// <returns>A Failure result instance containing the error.</returns>
    public static Result Failure(Error error)
    {
        Debug.Assert(error is not null, "Error cannot be null for Failure result.");

        Result result = new Failure(error);

        Debug.Assert(!result.IsSuccess, "Failure result must have IsSuccess == false.");
        Debug.Assert(result.IsFailure, "Failure result must have IsFailure == true.");

        return result;
    }

    /// <summary>
    /// Creates a successful result with a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value of the successful operation.</param>
    /// <returns>A Success&lt;T&gt; result instance containing the value.</returns>
    public static Result<T> Success<T>(T value)
    {
        Debug.Assert(value is not null, "Value cannot be null for Success result.");

        Result<T> result = new Success<T>(value);

        Debug.Assert(result.IsSuccess, "Success result must have IsSuccess == true.");
        Debug.Assert(!result.IsFailure, "Success result must have IsFailure == false.");

        return result;
    }

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <typeparam name="T">The type of the value that would have been returned on success.</typeparam>
    /// <param name="error">The error that caused the failure.</param>
    /// <returns>A Failure&lt;T&gt; result instance containing the error.</returns>
    public static Result<T> Failure<T>(Error error)
    {
        Debug.Assert(error is not null, "Error cannot be null for Failure result.");

        Result<T> result = new Failure<T>(error);

        Debug.Assert(!result.IsSuccess, "Failure result must have IsSuccess == false.");
        Debug.Assert(result.IsFailure, "Failure result must have IsFailure == true.");

        return result;
    }
}
