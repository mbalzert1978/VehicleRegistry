using System.Diagnostics;
using Shared.Kernel;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.Validation.Emails;

/// <summary>
/// Base class for email validation rules providing common success/failure creation logic.
/// </summary>
public abstract class EmailValidationRuleBase : IValidationRule<ValueObjects.Emails.Email>
{
    protected abstract string ErrorMessage { get; }

    public abstract Result Validate(ValueObjects.Emails.Email email);

    protected Result CreateSuccess()
    {
        Result success = ResultFactory.Success();
        Debug.Assert(success.IsSuccess, "Result should be a success");
        return success;
    }

    protected Result CreateFailure()
    {
        Error error = ErrorFactory.Validation("Email", ErrorMessage);
        Result failure = ResultFactory.Failure(error);
        Debug.Assert(failure.IsFailure, "Result should be a failure");
        return failure;
    }
}
