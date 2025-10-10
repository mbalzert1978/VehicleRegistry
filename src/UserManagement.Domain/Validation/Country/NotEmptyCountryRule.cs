using Shared.Kernel;

namespace UserManagement.Domain.Validation.Country;

public sealed class NotEmptyCountryRule : IValidationRule<ValueObjects.AddressComponents.Country>
{
    public Result Validate(ValueObjects.AddressComponents.Country country)
    {
        if (string.IsNullOrWhiteSpace(country.Value))
        {
            return ResultFactory.Failure(
                ErrorFactory.Validation(
                    nameof(ValueObjects.AddressComponents.Country),
                    "Country cannot be empty"
                )
            );
        }

        return ResultFactory.Success();
    }
}
