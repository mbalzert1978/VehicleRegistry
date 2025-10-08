using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Email;

/// <summary>
/// Validates that the email string is not null or whitespace.
/// </summary>
public sealed class EmailNotEmptyRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Email cannot be empty";

    public override Result Validate(string value) =>
        string.IsNullOrWhiteSpace(value) ? CreateFailure() : CreateSuccess();
}
