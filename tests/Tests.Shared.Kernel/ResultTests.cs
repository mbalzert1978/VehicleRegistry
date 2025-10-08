using Shared.Kernel;

namespace Tests.Shared.Kernel;

public sealed class ResultTests
{
    [Fact]
    public void Success_WhenCalled_ShouldReturnSuccessResult()
    {
        Result result = ResultFactory.Success();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Failure_WhenCalledWithError_ShouldReturnFailureResult()
    {
        Error error = ErrorFactory.Create("TEST_ERROR", "Test error message");

        Result result = ResultFactory.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }
}
