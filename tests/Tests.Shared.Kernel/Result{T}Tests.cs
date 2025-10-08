using Shared.Kernel;

namespace Tests.Shared.Kernel;

public sealed class ResultTTests
{
    [Fact]
    public void Success_WhenCalledWithValue_ShouldReturnSuccessResultWithValue()
    {
        const string expectedValue = "test value";

        Result<string> result = ResultFactory.Success(expectedValue);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void Failure_WhenCalledWithError_ShouldReturnFailureResult()
    {
        Error error = ErrorFactory.Create("TEST_ERROR", "Test error message");

        Result<string> result = ResultFactory.Failure<string>(error);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Value_WhenAccessedOnFailure_ShouldThrowInvalidOperationException()
    {
        Error error = ErrorFactory.Create("TEST_ERROR", "Test error message");
        Result<string> result = ResultFactory.Failure<string>(error);

        Func<string> act = () => result.Value;

        act.Should().ThrowExactly<InvalidOperationException>();
    }
}
