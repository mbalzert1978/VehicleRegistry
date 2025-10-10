using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Emails;

/// <summary>
/// Validates that the email string is not null or whitespace.
/// </summary>
public sealed class EmailNotEmptyRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Email cannot be empty";

    public override Result Validate(ValueObjects.Emails.Email email) =>
        string.IsNullOrWhiteSpace(email.Value) ? CreateFailure() : CreateSuccess();
}
