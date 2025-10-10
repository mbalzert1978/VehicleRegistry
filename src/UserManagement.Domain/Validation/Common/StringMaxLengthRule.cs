using System.Diagnostics;
using System.Globalization;
using System.Text;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Common;

/// <summary>
/// Validates that a string property extracted from a value object does not exceed a specified maximum length.
/// </summary>
/// <typeparam name="TValue">The value object type to validate.</typeparam>
public sealed class StringMaxLengthRule<TValue>(Func<TValue, string> selector, int maxLength)
    : IValidationRule<TValue>
    where TValue : class
{
    private const string ErrorTemplate = "{0} cannot exceed maximum length of {1} characters";
    private static readonly CompositeFormat Template = CompositeFormat.Parse(ErrorTemplate);

    public Result Validate(TValue value)
    {
        Debug.Assert(value is not null, "Value to validate must not be null");
        Debug.Assert(selector is not null, "Selector function must not be null");

        string input = selector.Invoke(value);

        if (input.Length > maxLength)
        {
            Error error = ErrorFactory.Validation(
                typeof(TValue).Name,
                string.Format(
                    CultureInfo.InvariantCulture,
                    Template,
                    typeof(TValue).Name,
                    maxLength
                )
            );
            Result failure = ResultFactory.Failure(error);

            Debug.Assert(failure.IsFailure, "Result should be a failure");

            return failure;
        }

        return ResultFactory.Success();
    }
}
