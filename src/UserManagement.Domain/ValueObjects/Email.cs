using System.Diagnostics;
using Shared.Kernel;
using UserManagement.Domain.Validation.Common;
using UserManagement.Domain.Validation.Emails;

namespace UserManagement.Domain.ValueObjects.Emails;

public sealed record Email(string Value);

public static class EmailFactory
{
    public static Result<Email> Create(string value, IValidationRule<Email>[]? rules = null)
    {
        Email email = new(value);

        rules ??=
        [
            new StringNotEmptyRule<Email>(e => e.Value),
            new ExactlyOneAtSymbolRule(),
            new NoWhitespaceRule(),
            new NoNewlineCharactersRule(),
            new NoTrailingDotRule(),
            new DomainNotEmptyRule(),
            new DomainNotStartWithDotRule(),
            new NoConsecutiveDotsInDomainRule(),
            new ValidDomainHyphensRule(),
            new NoUnderscoreInDomainRule(),
            new LocalPartMaxLengthRule(),
            new DomainPartMaxLengthRule(),
            new DomainLabelMaxLengthRule(),
        ];

        Debug.Assert(rules.Length > 0, "At least 1 validation rule must be provided");

        RuleComposer<Email> composedRule = RuleComposerFactory.Create(rules);
        Result validationResult = composedRule.Validate(email);

        if (validationResult is Failure failure)
        {
            Result<Email> failureResult = ResultFactory.Failure<Email>(failure.Error);
            Debug.Assert(failureResult.IsFailure, "Result should be a failure");
            return failureResult;
        }

        Debug.Assert(
            !string.IsNullOrWhiteSpace(email.Value),
            "Email value must not be empty after creation"
        );

        Result<Email> success = ResultFactory.Success(email);
        Debug.Assert(success.IsSuccess, "Result should be a success");
        Debug.Assert(success.Value == email, "Result value should be the created email");

        return success;
    }
}
