using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Emails;

/// <summary>
/// Validates that the email does not contain newline characters (\n, \r).
/// </summary>
public sealed class NoNewlineCharactersRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Email cannot contain newline characters";

    public override Result Validate(ValueObjects.Emails.Email email) =>
        (email.Value.Contains('\n') || email.Value.Contains('\r'))
            ? CreateFailure()
            : CreateSuccess();
}
