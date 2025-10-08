using Shared.Kernel;

namespace Tests.Shared.Kernel;

public sealed class ValidationRuleTests
{
    [Fact]
    public void Validate_WhenValueIsValid_ShouldReturnSuccess()
    {
        IValidationRule<string> rule = new TestValidationRule(isValid: true);
        const string testValue = "valid value";

        Result result = rule.Validate(testValue);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Validate_WhenValueIsInvalid_ShouldReturnFailure()
    {
        IValidationRule<string> rule = new TestValidationRule(isValid: false);
        const string testValue = "invalid value";

        Result result = rule.Validate(testValue);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    private sealed class TestValidationRule(bool isValid) : IValidationRule<string>
    {
        public Result Validate(string value) =>
            isValid
                ? ResultFactory.Success()
                : ResultFactory.Failure(ErrorFactory.Create("TEST_ERROR", "Validation failed"));
    }
}
