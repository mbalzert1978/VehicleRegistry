using Shared.Kernel;

namespace UserManagement.Domain.Validation.City;

public sealed class NotEmptyCityRule : IValidationRule<ValueObjects.AddressComponents.City>
{
    public Result Validate(ValueObjects.AddressComponents.City city)
    {
        if (string.IsNullOrWhiteSpace(city.Value))
        {
            return ResultFactory.Failure(
                ErrorFactory.Validation(
                    nameof(ValueObjects.AddressComponents.City),
                    "City cannot be empty"
                )
            );
        }

        return ResultFactory.Success();
    }
}
