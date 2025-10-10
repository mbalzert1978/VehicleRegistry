using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.ValueObjects;

public sealed record VerificationToken(string Token, DateTimeOffset ExpiresAt)
{
    public bool IsExpired(TimeProvider timeProvider) => timeProvider.GetUtcNow() >= ExpiresAt;
}

public static class VerificationTokenFactory
{
    public static Result<VerificationToken> Create(
        string token,
        DateTimeOffset expiresAt,
        TimeProvider timeProvider
    )
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return CreateError(nameof(VerificationToken.Token), "Token cannot be empty");
        }

        DateTimeOffset now = timeProvider.GetUtcNow();
        if (expiresAt <= now)
        {
            return CreateError(
                nameof(VerificationToken.ExpiresAt),
                "ExpiresAt must be in the future"
            );
        }

        var verificationToken = new VerificationToken(token, expiresAt);

        Debug.Assert(
            !string.IsNullOrWhiteSpace(verificationToken.Token),
            "Token must not be empty after creation"
        );
        Debug.Assert(
            verificationToken.ExpiresAt > timeProvider.GetUtcNow(),
            "ExpiresAt must be in the future after creation"
        );

        Result<VerificationToken> success = ResultFactory.Success(verificationToken);
        Debug.Assert(success.IsSuccess, "Result should be a success");
        Debug.Assert(
            success.Value == verificationToken,
            "Result value should be the created verification token"
        );

        return success;
    }

    private static Result<VerificationToken> CreateError(string field, string message)
    {
        Error error = ErrorFactory.Validation(field, message);
        Result<VerificationToken> failure = ResultFactory.Failure<VerificationToken>(error);
        Debug.Assert(failure.IsFailure, "Result should be a failure");
        return failure;
    }
}
