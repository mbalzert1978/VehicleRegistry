using Shared.Kernel;

namespace Tests.Shared.Kernel;

public sealed class RequestHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenCalled_ShouldReturnResult()
    {
        TestRequest request = new("test data");
        IRequestHandler<TestRequest, TestResponse> handler = new TestRequestHandler();

        Result<TestResponse> result = await handler.HandleAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.Message.Should().Be("Processed: test data");
    }

    [Fact]
    public async Task HandleAsync_WhenHandlerFails_ShouldReturnFailure()
    {
        TestRequest request = new("fail");
        IRequestHandler<TestRequest, TestResponse> handler = new TestRequestHandler();

        Result<TestResponse> result = await handler.HandleAsync(request);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    private sealed record TestRequest(string Data);

    private sealed record TestResponse(string Message);

    private sealed class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
    {
        public Task<Result<TestResponse>> HandleAsync(TestRequest request) =>
            request switch
            {
                { Data: "fail" } => Task.FromResult(
                    ResultFactory.Failure<TestResponse>(
                        ErrorFactory.Create("TEST_ERROR", "Handler failed")
                    )
                ),
                _ => Task.FromResult(
                    ResultFactory.Success<TestResponse>(new($"Processed: {request.Data}"))
                ),
            };
    }
}
