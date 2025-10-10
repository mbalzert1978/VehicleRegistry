using Shared.Kernel;

namespace UserManagement.Domain.Validation.PostalCode;

public sealed class MaxLengthPostalCodeRule(int maxLength)
    : IValidationRule<ValueObjects.AddressComponents.PostalCode>
{
    public Result Validate(ValueObjects.AddressComponents.PostalCode postalCode)
    {
        if (postalCode.Value.Length > maxLength)
        {
            return ResultFactory.Failure(
                ErrorFactory.Validation(
                    nameof(ValueObjects.AddressComponents.PostalCode),
                    $"PostalCode cannot exceed maximum length of {maxLength} characters"
                )
            );
        }

        return ResultFactory.Success();
    }
}
