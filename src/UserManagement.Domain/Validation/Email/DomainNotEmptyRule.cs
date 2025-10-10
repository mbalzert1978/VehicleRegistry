using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Emails;

/// <summary>
/// Validation rule ensuring email domain part is not empty.
/// </summary>
public sealed class DomainNotEmptyRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Email domain cannot be empty";

    public override Result Validate(ValueObjects.Emails.Email email) =>
        email.Value.Split('@') switch
        {
            [_, var domain] when !string.IsNullOrEmpty(domain) => CreateSuccess(),
            _ => CreateFailure(),
        };
}
