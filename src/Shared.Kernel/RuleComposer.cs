using System.Collections.Immutable;

namespace Shared.Kernel;

/// <summary>
/// Composes multiple validation rules using the Composite pattern.
/// Executes rules in sequence and returns the first failure, or success if all pass.
/// </summary>
/// <typeparam name="TRuleFor">The type of value to validate.</typeparam>
/// <param name="Rules">The collection of validation rules to compose.</param>
public sealed record RuleComposer<TRuleFor>(ImmutableList<IValidationRule<TRuleFor>> Rules) : IValidationRule<TRuleFor>
{
    /// <summary>
    /// Validates the specified value against all composed rules.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns>
    /// A <see cref="Result"/> indicating success if all rules pass,
    /// or the first failure encountered.
    /// </returns>
    public Result Validate(TRuleFor value) =>
        Rules.Aggregate(
            ResultFactory.Success(),
            (current, rule) => current.IsFailure ? current : rule.Validate(value));
}

/// <summary>
/// Factory for creating RuleComposer instances.
/// </summary>
public static class RuleComposerFactory
{
    /// <summary>
    /// Creates a new rule composer with the specified rules.
    /// </summary>
    /// <typeparam name="T">The type of value to validate.</typeparam>
    /// <param name="rules">The validation rules to compose.</param>
    /// <returns>A new <see cref="RuleComposer{T}"/> instance.</returns>
    public static RuleComposer<T> Create<T>(params IValidationRule<T>[] rules) =>
        new([.. rules]);
}
