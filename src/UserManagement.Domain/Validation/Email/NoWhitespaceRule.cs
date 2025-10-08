using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Email;

/// <summary>
/// Validates that the email does not contain whitespace characters.
/// </summary>
public sealed class NoWhitespaceRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Email cannot contain whitespace characters";

    public override Result Validate(string value) =>
        value.Any(char.IsWhiteSpace) ? CreateFailure() : CreateSuccess();
}
