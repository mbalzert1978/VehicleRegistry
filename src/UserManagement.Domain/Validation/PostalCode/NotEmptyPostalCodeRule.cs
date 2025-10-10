using Shared.Kernel;

namespace UserManagement.Domain.Validation.PostalCode;

public sealed class NotEmptyPostalCodeRule
    : IValidationRule<ValueObjects.AddressComponents.PostalCode>
{
    public Result Validate(ValueObjects.AddressComponents.PostalCode postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode.Value))
        {
            return ResultFactory.Failure(
                ErrorFactory.Validation(
                    nameof(ValueObjects.AddressComponents.PostalCode),
                    "PostalCode cannot be empty"
                )
            );
        }

        return ResultFactory.Success();
    }
}
