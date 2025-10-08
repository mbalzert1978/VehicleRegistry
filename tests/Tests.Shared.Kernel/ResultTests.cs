using Shared.Kernel;

namespace Tests.Shared.Kernel;


public sealed class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        Result result = ResultFactory.Success();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }
}
