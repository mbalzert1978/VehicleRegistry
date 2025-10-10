using Shared.Kernel;

namespace UserManagement.Domain.Validation.Street;

public sealed class NotEmptyStreetRule : IValidationRule<ValueObjects.AddressComponents.Street>
{
    public Result Validate(ValueObjects.AddressComponents.Street street)
    {
        if (string.IsNullOrWhiteSpace(street.Value))
        {
            return ResultFactory.Failure(
                ErrorFactory.Validation(
                    nameof(ValueObjects.AddressComponents.Street),
                    "Street cannot be empty"
                )
            );
        }

        return ResultFactory.Success();
    }
}
