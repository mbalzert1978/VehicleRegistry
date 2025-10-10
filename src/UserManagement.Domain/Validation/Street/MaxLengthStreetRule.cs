using Shared.Kernel;

namespace UserManagement.Domain.Validation.Street;

public sealed class MaxLengthStreetRule(int maxLength)
    : IValidationRule<ValueObjects.AddressComponents.Street>
{
    public Result Validate(ValueObjects.AddressComponents.Street street)
    {
        if (street.Value.Length > maxLength)
        {
            return ResultFactory.Failure(
                ErrorFactory.Validation(
                    nameof(ValueObjects.AddressComponents.Street),
                    $"Street cannot exceed maximum length of {maxLength} characters"
                )
            );
        }

        return ResultFactory.Success();
    }
}
