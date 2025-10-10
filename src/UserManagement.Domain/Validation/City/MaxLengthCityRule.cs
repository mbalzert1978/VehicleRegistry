using Shared.Kernel;

namespace UserManagement.Domain.Validation.City;

public sealed class MaxLengthCityRule(int maxLength)
    : IValidationRule<ValueObjects.AddressComponents.City>
{
    public Result Validate(ValueObjects.AddressComponents.City city)
    {
        if (city.Value.Length > maxLength)
        {
            return ResultFactory.Failure(
                ErrorFactory.Validation(
                    nameof(ValueObjects.AddressComponents.City),
                    $"City cannot exceed maximum length of {maxLength} characters"
                )
            );
        }

        return ResultFactory.Success();
    }
}
