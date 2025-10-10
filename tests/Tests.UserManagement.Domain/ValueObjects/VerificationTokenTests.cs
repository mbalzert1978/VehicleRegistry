using Microsoft.Extensions.Time.Testing;
using Shared.Kernel;
using UserManagement.Domain.ValueObjects;

namespace Tests.UserManagement.Domain.ValueObjects;

public sealed class VerificationTokenTests
{
    private static readonly DateTimeOffset FixedTime = new(2025, 10, 10, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ExpiresAt = FixedTime.AddHours(1);
    private readonly FakeTimeProvider _timeProvider = new(FixedTime);

    [Fact]
    public void Create_WithValidTokenAndExpiration_ShouldSucceed()
    {
        string token = Guid.NewGuid().ToString();

        Result<VerificationToken> result = VerificationTokenFactory.Create(
            token,
            ExpiresAt,
            _timeProvider
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.Token.Should().Be(token);
        result.Value.ExpiresAt.Should().Be(ExpiresAt);
    }

    [Fact]
    public void Create_WithEmptyToken_ShouldFail()
    {
        string token = string.Empty;

        Result<VerificationToken> result = VerificationTokenFactory.Create(
            token,
            ExpiresAt,
            _timeProvider
        );

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<VerificationToken>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("TOKEN.Validation");
        error.Message.Should().Be("Token cannot be empty");
    }

    [Fact]
    public void Create_WithWhitespaceToken_ShouldFail()
    {
        string token = "   ";

        Result<VerificationToken> result = VerificationTokenFactory.Create(
            token,
            ExpiresAt,
            _timeProvider
        );

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<VerificationToken>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("TOKEN.Validation");
        error.Message.Should().Be("Token cannot be empty");
    }

    [Fact]
    public void Create_WithExpirationInPast_ShouldFail()
    {
        string token = Guid.NewGuid().ToString();
        DateTimeOffset expiresAt = FixedTime.AddHours(-1);

        Result<VerificationToken> result = VerificationTokenFactory.Create(
            token,
            expiresAt,
            _timeProvider
        );

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<VerificationToken>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("EXPIRESAT.Validation");
        error.Message.Should().Be("ExpiresAt must be in the future");
    }

    [Fact]
    public void IsExpired_WhenExpirationInFuture_ShouldReturnFalse()
    {
        string token = Guid.NewGuid().ToString();

        Result<VerificationToken> result = VerificationTokenFactory.Create(
            token,
            ExpiresAt,
            _timeProvider
        );

        result.Value.IsExpired(_timeProvider).Should().BeFalse();
    }

    [Fact]
    public void IsExpired_WhenExpirationInPast_ShouldReturnTrue()
    {
        string token = Guid.NewGuid().ToString();

        Result<VerificationToken> result = VerificationTokenFactory.Create(
            token,
            ExpiresAt,
            _timeProvider
        );

        _timeProvider.Advance(TimeSpan.FromHours(2));

        result.Value.IsExpired(_timeProvider).Should().BeTrue();
    }

    [Fact]
    public void Create_WithNullToken_ShouldFail()
    {
        string? token = null;

        Result<VerificationToken> result = VerificationTokenFactory.Create(
            token!,
            ExpiresAt,
            _timeProvider
        );

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<VerificationToken>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("TOKEN.Validation");
        error.Message.Should().Be("Token cannot be empty");
    }

    [Fact]
    public void RecordEquality_WithSameValues_ShouldBeEqual()
    {
        string token = Guid.NewGuid().ToString();

        Result<VerificationToken> result1 = VerificationTokenFactory.Create(
            token,
            ExpiresAt,
            _timeProvider
        );
        Result<VerificationToken> result2 = VerificationTokenFactory.Create(
            token,
            ExpiresAt,
            _timeProvider
        );

        result1.Value.Should().Be(result2.Value);
    }

    [Fact]
    public void RecordEquality_WithDifferentTokens_ShouldNotBeEqual()
    {
        string token1 = Guid.NewGuid().ToString();
        string token2 = Guid.NewGuid().ToString();

        Result<VerificationToken> result1 = VerificationTokenFactory.Create(
            token1,
            ExpiresAt,
            _timeProvider
        );
        Result<VerificationToken> result2 = VerificationTokenFactory.Create(
            token2,
            ExpiresAt,
            _timeProvider
        );

        result1.Value.Should().NotBe(result2.Value);
    }

    [Fact]
    public void RecordEquality_WithDifferentExpirations_ShouldNotBeEqual()
    {
        string token = Guid.NewGuid().ToString();
        DateTimeOffset expiresAt1 = FixedTime.AddHours(24);
        DateTimeOffset expiresAt2 = FixedTime.AddHours(48);

        Result<VerificationToken> result1 = VerificationTokenFactory.Create(
            token,
            expiresAt1,
            _timeProvider
        );
        Result<VerificationToken> result2 = VerificationTokenFactory.Create(
            token,
            expiresAt2,
            _timeProvider
        );

        result1.Value.Should().NotBe(result2.Value);
    }

    [Fact]
    public void Immutability_TokenAndExpiresAtShouldBeReadOnly()
    {
        string token = Guid.NewGuid().ToString();

        Result<VerificationToken> result = VerificationTokenFactory.Create(
            token,
            ExpiresAt,
            _timeProvider
        );

        VerificationToken verificationToken = result.Value;
        verificationToken.Token.Should().Be(token);
        verificationToken.ExpiresAt.Should().Be(ExpiresAt);
    }

    [Fact]
    public void Create_WithVeryLongToken_ShouldSucceed()
    {
        string token = new string('a', 500);

        Result<VerificationToken> result = VerificationTokenFactory.Create(
            token,
            ExpiresAt,
            _timeProvider
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.Token.Should().HaveLength(500);
    }
}
