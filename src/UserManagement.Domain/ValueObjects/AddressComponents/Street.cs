using System.Diagnostics;
using Shared.Kernel;
using UserManagement.Domain.Validation.Common;

namespace UserManagement.Domain.ValueObjects.AddressComponents;

public sealed record Street(string Value);

public static class StreetFactory
{
    private const int MaxLength = 200;

    public static Result<Street> Create(string value, IValidationRule<Street>[]? rules = null)
    {
        Street street = new(value);

        rules ??=
        [
            new StringNotEmptyRule<Street>(s => s.Value),
            new StringMaxLengthRule<Street>(s => s.Value, MaxLength),
        ];

        Debug.Assert(rules.Length > 0, "At least 1 validation rule must be provided");

        RuleComposer<Street> composedRule = RuleComposerFactory.Create(rules);
        Result validationResult = composedRule.Validate(street);

        if (validationResult is Failure failure)
        {
            Result<Street> failureResult = ResultFactory.Failure<Street>(failure.Error);
            Debug.Assert(failureResult.IsFailure, "Result should be a failure");
            return failureResult;
        }

        Debug.Assert(
            !string.IsNullOrWhiteSpace(street.Value),
            "Street value must not be empty after creation"
        );
        Debug.Assert(
            street.Value.Length <= MaxLength,
            "Street value must not exceed maximum length after creation"
        );

        Result<Street> success = ResultFactory.Success(street);
        Debug.Assert(success.IsSuccess, "Result should be a success");
        Debug.Assert(success.Value == street, "Result value should be the created street");

        return success;
    }
}
