using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Email;

/// <summary>
/// Validates that the local part (before @) does not exceed 64 characters (RFC 5321).
/// </summary>
public sealed class LocalPartMaxLengthRule : EmailValidationRuleBase
{
    private const int MaxLocalPartLength = 64;
    protected override string ErrorMessage =>
        $"Email local part cannot exceed {MaxLocalPartLength} characters";

    public override Result Validate(string value) =>
        value.Split('@') switch
        {
            [var localPart, _] when localPart.Length <= MaxLocalPartLength => CreateSuccess(),
            _ => CreateFailure(),
        };
}
