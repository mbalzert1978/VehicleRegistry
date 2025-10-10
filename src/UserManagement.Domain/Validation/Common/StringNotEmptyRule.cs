using System.Diagnostics;
using System.Globalization;
using System.Text;
using Shared.Kernel;

namespace UserManagement.Domain.Validation.Common;

/// <summary>
/// Validates that a string property extracted from a value object is not null, empty, or whitespace.
/// </summary>
/// <typeparam name="TValue">The value object type to validate.</typeparam>
public sealed class StringNotEmptyRule<TValue>(Func<TValue, string> selector)
    : IValidationRule<TValue>
    where TValue : class
{
    private const string ErrorTemplate = "{0} cannot be empty";
    private static readonly CompositeFormat Template = CompositeFormat.Parse(ErrorTemplate);

    public Result Validate(TValue value)
    {
        Debug.Assert(value is not null, "Value to validate must not be null");
        Debug.Assert(selector is not null, "Selector function must not be null");

        string input = selector.Invoke(value);

        if (string.IsNullOrWhiteSpace(input))
        {
            Error error = ErrorFactory.Validation(
                typeof(TValue).Name,
                string.Format(CultureInfo.InvariantCulture, Template, typeof(TValue).Name)
            );
            Result failure = ResultFactory.Failure(error);

            Debug.Assert(failure.IsFailure, "Result should be a failure");

            return failure;
        }

        Result success = ResultFactory.Success();
        Debug.Assert(success.IsSuccess, "Result should be a success");

        return success;
    }
}
