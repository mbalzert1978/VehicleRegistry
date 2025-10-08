using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Email;

/// <summary>
/// Validates that the email contains exactly one @ symbol.
/// </summary>
public sealed class ExactlyOneAtSymbolRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Email must contain exactly one @ symbol";

    public override Result Validate(string value) =>
        value.Count(c => c == '@') switch
        {
            1 => CreateSuccess(),
            _ => CreateFailure(),
        };
}
