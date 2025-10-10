using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.ValueObjects;

public sealed record Email(string Value);

public static class EmailFactory
{
    public static Result<Email> Create(string value, IValidationRule<string>[] rules)
    {
        Debug.Assert(rules is not null, "Validation rules must not be null");
        Debug.Assert(rules.Length > 0, "At least one validation rule must be provided");

        RuleComposer<string> composedRule = RuleComposerFactory.Create(rules);
        Result validationResult = composedRule.Validate(value);

        return validationResult switch
        {
            Success => CreateEmail(value),
            Failure failure => ResultFactory.Failure<Email>(failure.Error),
            _ => throw new InvalidOperationException("Unknown Result type"),
        };
    }

    private static Result<Email> CreateEmail(string value)
    {
        Email email = new(value);

        Debug.Assert(email.Value == value, "Email value must match input");
        Debug.Assert(
            !string.IsNullOrWhiteSpace(email.Value),
            "Email value must not be empty after creation"
        );

        return ResultFactory.Success(email);
    }
}
