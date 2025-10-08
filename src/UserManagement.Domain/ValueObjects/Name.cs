using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.ValueObjects;

/// <summary>
/// Represents a person's name with first and last name.
/// </summary>
public sealed record Name(string FirstName, string LastName);

/// <summary>
/// Factory for creating Name instances with validation.
/// </summary>
public static class NameFactory
{
    private const int MaxNameLength = 100;
    private const string NotEmpty = " cannot be empty";
    private static readonly string MaxLengthExceeded =
        $" cannot exceed maximum length of {MaxNameLength} characters";

    public static Result<Name> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return CreateError(nameof(Name.FirstName), nameof(Name.FirstName) + NotEmpty);
        if (firstName.Length > MaxNameLength)
            return CreateError(nameof(Name.FirstName), nameof(Name.FirstName) + MaxLengthExceeded);
        if (string.IsNullOrWhiteSpace(lastName))
            return CreateError(nameof(Name.LastName), nameof(Name.LastName) + NotEmpty);
        if (lastName.Length > MaxNameLength)
            return CreateError(nameof(Name.LastName), nameof(Name.LastName) + MaxLengthExceeded);

        Name name = new(firstName, lastName);
        Debug.Assert(name.FirstName == firstName, "FirstName should match");
        Debug.Assert(name.LastName == lastName, "LastName should match");

        Result<Name> success = ResultFactory.Success(name);
        Debug.Assert(success.IsSuccess, "Result should be a success");
        Debug.Assert(success.Value == name, "Result value should be the created name");
        return success;
    }

    private static Result<Name> CreateError(string field, string message)
    {
        Error error = ErrorFactory.Validation(field, message);
        Result<Name> failure = ResultFactory.Failure<Name>(error);
        Debug.Assert(failure.IsFailure, "Result should be a failure");
        return failure;
    }
}
