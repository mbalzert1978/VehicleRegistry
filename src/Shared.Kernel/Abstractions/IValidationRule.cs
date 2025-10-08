namespace Shared.Kernel;

/// <summary>
/// Represents a validation rule for a value of type <typeparamref name="TRuleFor"/>.
/// </summary>
/// <typeparam name="TRuleFor">The type of value to validate.</typeparam>
public interface IValidationRule<in TRuleFor>
{
    /// <summary>
    /// Validates the specified value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure with error details.</returns>
    Result Validate(TRuleFor value);
}
