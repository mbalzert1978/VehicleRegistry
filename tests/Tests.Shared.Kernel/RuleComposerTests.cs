using Shared.Kernel;

namespace Tests.Shared.Kernel;

public sealed class RuleComposerTests
{
    [Fact]
    public void Validate_WhenAllRulesPass_ShouldReturnSuccess()
    {
        IValidationRule<string> rule1 = new PassingRule();
        IValidationRule<string> rule2 = new PassingRule();
        RuleComposer<string> composer = new([rule1, rule2]);
        const string testValue = "test";

        Result result = composer.Validate(testValue);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Validate_WhenOneRuleFails_ShouldReturnFailure()
    {
        IValidationRule<string> rule1 = new PassingRule();
        IValidationRule<string> rule2 = new FailingRule();
        RuleComposer<string> composer = new([rule1, rule2]);
        const string testValue = "test";

        Result result = composer.Validate(testValue);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenFirstRuleFails_ShouldStopAndReturnFailure()
    {
        IValidationRule<string> rule1 = new FailingRule();
        IValidationRule<string> rule2 = new PassingRule();
        RuleComposer<string> composer = new([rule1, rule2]);
        const string testValue = "test";

        Result result = composer.Validate(testValue);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenNoRules_ShouldReturnSuccess()
    {
        RuleComposer<string> composer = new([]);
        const string testValue = "test";

        Result result = composer.Validate(testValue);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    private sealed class PassingRule : IValidationRule<string>
    {
        public Result Validate(string value) => ResultFactory.Success();
    }

    private sealed class FailingRule : IValidationRule<string>
    {
        public Result Validate(string value) =>
            ResultFactory.Failure(ErrorFactory.Create("FAIL", "Rule failed"));
    }
}
