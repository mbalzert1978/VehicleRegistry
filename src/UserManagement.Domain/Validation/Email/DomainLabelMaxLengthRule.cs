using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Email;

/// <summary>
/// Validates that domain labels do not exceed 63 characters (RFC 1034).
/// </summary>
public sealed class DomainLabelMaxLengthRule : EmailValidationRuleBase
{
    private const int MaxLabelLength = 63;
    protected override string ErrorMessage =>
        $"Domain labels cannot exceed {MaxLabelLength} characters";

    public override Result Validate(string value) =>
        value.Split('@') switch
        {
            [_, var domain] when domain.Split('.').All(label => label.Length <= MaxLabelLength) =>
                CreateSuccess(),
            _ => CreateFailure(),
        };
}
