using Shared.Kernel;

namespace UserManagement.Domain.Validation.City;

public sealed class MaxLengthRule<TValue>(Func<TValue, string> selector, int maxLength)
    : IValidationRule<TValue>
    where TValue : class
{
    public Result Validate(TValue value)
    {
        string input = selector.Invoke(value);

        if (input.Length > maxLength)
            return ResultFactory.Failure(
                ErrorFactory.Validation(
                    typeof(TValue).Name,
                    $"{typeof(TValue).Name} cannot exceed maximum length of {maxLength} characters"
                )
            );

        return ResultFactory.Success();
    }
}
