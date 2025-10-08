namespace Shared.Kernel;

/// <summary>
/// Represents a handler for a request/response operation.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
{
    /// <summary>
    /// Handles the specified request asynchronously.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="Result{TResponse}"/> indicating success with the response or failure with error details.
    /// </returns>
    Task<Result<TResponse>> HandleAsync(TRequest request);
}
