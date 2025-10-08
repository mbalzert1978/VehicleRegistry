using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Email;

/// <summary>
/// Validates that the domain does not start or end with a hyphen, and labels don't start/end with hyphens.
/// </summary>
public sealed class ValidDomainHyphensRule : EmailValidationRuleBase
{
    protected override string ErrorMessage => "Domain labels cannot start or end with hyphen";

    public override Result Validate(string value) =>
        value.Split('@') switch
        {
            [_, var domain]
                when domain
                    .Split('.')
                    .All(label => !label.StartsWith('-') && !label.EndsWith('-')) =>
                CreateSuccess(),
            _ => CreateFailure(),
        };
}
