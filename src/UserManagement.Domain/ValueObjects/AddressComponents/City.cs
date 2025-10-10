using System.Diagnostics;
using Shared.Kernel;
using UserManagement.Domain.Validation.City;

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
            new NotEmptyRule<City>(c => c.Value),
            new MaxLengthRule<City>(c => c.Value, MaxLength),
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

        Debug.Assert(
            !string.IsNullOrWhiteSpace(city.Value),
            "City value must not be empty after creation"
        );
        Debug.Assert(
            city.Value.Length <= MaxLength,
            "City value must not exceed maximum length after creation"
        );

        Result<City> success = ResultFactory.Success(city);
        Debug.Assert(success.IsSuccess, "Result should be a success");
        Debug.Assert(success.Value == city, "Result value should be the created city");

        return success;
    }
}
