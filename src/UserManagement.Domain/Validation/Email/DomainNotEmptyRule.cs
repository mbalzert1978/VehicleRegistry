using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Email;

/// <summary>
/// Validation rule ensuring email domain part is not empty.
/// </summary>
public sealed class DomainNotEmptyRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Email domain cannot be empty";

    public override Result Validate(string value) =>
        value.Split('@') switch
        {
            [_, var domain] when !string.IsNullOrEmpty(domain) => CreateSuccess(),
            _ => CreateFailure(),
        };
}
