using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Email;

/// <summary>
/// Validates that the domain does not start with a dot.
/// </summary>
public sealed class DomainNotStartWithDotRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Domain cannot start with a dot";

    public override Result Validate(string value) =>
        value.Split('@') switch
        {
            [_, var domain] when !domain.StartsWith('.') => CreateSuccess(),
            _ => CreateFailure(),
        };
}
