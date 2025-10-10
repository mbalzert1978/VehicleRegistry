using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Emails;

/// <summary>
/// Validates that the domain does not contain consecutive dots.
/// </summary>
public sealed class NoConsecutiveDotsInDomainRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Domain cannot contain consecutive dots";

    public override Result Validate(ValueObjects.Emails.Email email) =>
        email.Value.Split('@') switch
        {
            [_, var domain] when !domain.Contains("..") => CreateSuccess(),
            _ => CreateFailure(),
        };
}
