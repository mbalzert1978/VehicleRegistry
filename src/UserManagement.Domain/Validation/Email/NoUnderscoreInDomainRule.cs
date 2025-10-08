using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Email;

/// <summary>
/// Validates that the domain part does not contain underscore characters.
/// </summary>
public sealed class NoUnderscoreInDomainRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Email domain cannot contain underscore characters";

    public override Result Validate(string value) =>
        value.Split('@') switch
        {
            [_, var domain] when !domain.Contains('_') => CreateSuccess(),
            _ => CreateFailure(),
        };
}
