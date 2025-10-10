using Shared.Kernel;

namespace UserManagement.Domain.Validation.Country;

public sealed class MaxLengthCountryRule(int maxLength)
    : IValidationRule<ValueObjects.AddressComponents.Country>
{
    public Result Validate(ValueObjects.AddressComponents.Country country)
    {
        if (country.Value.Length > maxLength)
        {
            return ResultFactory.Failure(
                ErrorFactory.Validation(
                    nameof(ValueObjects.AddressComponents.Country),
                    $"Country cannot exceed maximum length of {maxLength} characters"
                )
            );
        }

        return ResultFactory.Success();
    }
}
