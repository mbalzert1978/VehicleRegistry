using Shared.Kernel;
using Xunit;

namespace Tests.Shared.Kernel;


public sealed class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        Result result = Result.Success();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }
}
