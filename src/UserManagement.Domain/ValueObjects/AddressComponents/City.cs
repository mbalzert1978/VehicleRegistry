using System.Diagnostics;
using Shared.Kernel;
using UserManagement.Domain.Validation.Common;

namespace UserManagement.Domain.ValueObjects.AddressComponents;

public sealed record City(string Value);

public static class CityFactory
{
    private const int MaxLength = 100;

    public static Result<City> Create(string value, IValidationRule<City>[]? rules = null)
    {
        City city = new(value);

        rules ??=
        [
            new StringNotEmptyRule<City>(c => c.Value),
            new StringMaxLengthRule<City>(c => c.Value, MaxLength),
        ];

        Debug.Assert(rules.Length > 0, "At least 1 validation rule must be provided");

        RuleComposer<City> composedRule = RuleComposerFactory.Create(rules);
        Result validationResult = composedRule.Validate(city);

        if (validationResult is Failure failure)
        {
            Result<City> failureResult = ResultFactory.Failure<City>(failure.Error);
            Debug.Assert(failureResult.IsFailure, "Result should be a failure");
            return failureResult;
        }
        Debug.Assert(validationResult.IsSuccess, "Validation result should be a success");

        return ResultFactory.Success(city);
    }
}
