using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Emails;

/// <summary>
/// Validates that the email does not contain whitespace characters.
/// </summary>
public sealed class NoWhitespaceRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Email cannot contain whitespace characters";

    public override Result Validate(ValueObjects.Emails.Email email) =>
        email.Value.Any(char.IsWhiteSpace) ? CreateFailure() : CreateSuccess();
}
