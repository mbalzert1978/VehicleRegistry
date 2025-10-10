using Shared.Kernel;

namespace UserManagement.Domain.Validation.City;

public sealed class NotEmptyRule<TValue>(Func<TValue, string> selector) : IValidationRule<TValue>
    where TValue : class
{
    public Result Validate(TValue value)
    {
        string input = selector.Invoke(value);

        if (string.IsNullOrWhiteSpace(input))
            return ResultFactory.Failure(
                ErrorFactory.Validation(
                    typeof(TValue).Name,
                    $"{typeof(TValue).Name} cannot be empty"
                )
            );

        return ResultFactory.Success();
    }
}
