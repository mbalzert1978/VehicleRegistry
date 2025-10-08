using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Email;

/// <summary>
/// Validates that the email does not contain newline characters (\n, \r).
/// </summary>
public sealed class NoNewlineCharactersRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Email cannot contain newline characters";

    public override Result Validate(string value) =>
        (value.Contains('\n') || value.Contains('\r')) ? CreateFailure() : CreateSuccess();
}
