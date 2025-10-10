using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Emails;

/// <summary>
/// Validates that the domain part (after @) does not exceed 255 characters (RFC 5321).
/// </summary>
public sealed class DomainPartMaxLengthRule : EmailValidationRuleBase
{
    private const int MaxDomainPartLength = 255;
    protected override string ErrorMessage =>
        $"Email domain part cannot exceed {MaxDomainPartLength} characters";

    public override Result Validate(ValueObjects.Emails.Email email) =>
        email.Value.Split('@') switch
        {
            [_, var domain] when domain.Length <= MaxDomainPartLength => CreateSuccess(),
            _ => CreateFailure(),
        };
}
