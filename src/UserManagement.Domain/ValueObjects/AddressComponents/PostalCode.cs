using System.Diagnostics;
using Shared.Kernel;
using UserManagement.Domain.Validation.City;

namespace UserManagement.Domain.ValueObjects.AddressComponents;

public sealed record PostalCode(string Value);

public static class PostalCodeFactory
{
    private const int MaxLength = 20;

    public static Result<PostalCode> Create(
        string value,
        IValidationRule<PostalCode>[]? rules = null
    )
    {
        PostalCode postalCode = new(value);

        rules ??=
        [
            new NotEmptyRule<PostalCode>(c => c.Value),
            new MaxLengthRule<PostalCode>(c => c.Value, MaxLength),
        ];

        Debug.Assert(rules.Length > 0, "At least 1 validation rule must be provided");

        RuleComposer<PostalCode> composedRule = RuleComposerFactory.Create(rules);
        Result validationResult = composedRule.Validate(postalCode);

        if (validationResult is Failure failure)
        {
            Result<PostalCode> failureResult = ResultFactory.Failure<PostalCode>(failure.Error);
            Debug.Assert(failureResult.IsFailure, "Result should be a failure");
            return failureResult;
        }

        Debug.Assert(
            !string.IsNullOrWhiteSpace(postalCode.Value),
            "PostalCode value must not be empty after creation"
        );
        Debug.Assert(
            postalCode.Value.Length <= MaxLength,
            "PostalCode value must not exceed maximum length after creation"
        );

        Result<PostalCode> success = ResultFactory.Success(postalCode);
        Debug.Assert(success.IsSuccess, "Result should be a success");
        Debug.Assert(success.Value == postalCode, "Result value should be the created postal code");

        return success;
    }
}
