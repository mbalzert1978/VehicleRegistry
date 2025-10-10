using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Emails;

/// <summary>
/// Validates that the email does not end with a trailing dot.
/// </summary>
public sealed class NoTrailingDotRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Email cannot end with a dot";

    public override Result Validate(ValueObjects.Emails.Email email) =>
        email.Value.EndsWith('.') ? CreateFailure() : CreateSuccess();
}
