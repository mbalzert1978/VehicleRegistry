using System.Diagnostics;
using Shared.Kernel;
using UserManagement.Domain.Validation.Common;

namespace UserManagement.Domain.ValueObjects.AddressComponents;

public sealed record Country(string Value);

public static class CountryFactory
{
    private const int MaxLength = 100;

    public static Result<Country> Create(string value, IValidationRule<Country>[]? rules = null)
    {
        Country country = new(value);

        rules ??=
        [
            new StringNotEmptyRule<Country>(c => c.Value),
            new StringMaxLengthRule<Country>(c => c.Value, MaxLength),
        ];

        Debug.Assert(rules.Length > 0, "At least 1 validation rule must be provided");

        RuleComposer<Country> composedRule = RuleComposerFactory.Create(rules);
        Result validationResult = composedRule.Validate(country);

        if (validationResult is Failure failure)
        {
            Result<Country> failureResult = ResultFactory.Failure<Country>(failure.Error);
            Debug.Assert(failureResult.IsFailure, "Result should be a failure");
            return failureResult;
        }
        Debug.Assert(validationResult.IsSuccess, "Validation result should be a success");

        return ResultFactory.Success(country);
    }
}
