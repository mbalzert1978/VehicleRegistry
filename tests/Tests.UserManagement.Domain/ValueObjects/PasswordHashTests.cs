using AwesomeAssertions;
using Shared.Kernel;
using UserManagement.Domain.ValueObjects;

namespace Tests.UserManagement.Domain.ValueObjects;

public sealed class PasswordHashTests
{
    [Fact]
    public void Create_WithValidHash_ShouldSucceed()
    {
        const string validHash = "$argon2id$v=19$m=65536,t=3,p=4$c29tZXNhbHQ$hash123";

        Result<PasswordHash> result = PasswordHashFactory.Create(validHash);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(validHash);
    }

    [Fact]
    public void Create_WithEmptyHash_ShouldFail()
    {
        const string emptyHash = "";

        Result<PasswordHash> result = PasswordHashFactory.Create(emptyHash);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<PasswordHash>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure")
        };

        error.Code.Should().Be("PASSWORDHASH.Validation");
    }

    [Fact]
    public void Create_WithWhitespaceHash_ShouldFail()
    {
        const string whitespaceHash = "   ";

        Result<PasswordHash> result = PasswordHashFactory.Create(whitespaceHash);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<PasswordHash>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure")
        };

        error.Code.Should().Be("PASSWORDHASH.Validation");
    }

    [Fact]
    public void Create_WithNullHash_ShouldFail()
    {
        string? nullHash = null;

        Result<PasswordHash> result = PasswordHashFactory.Create(nullHash!);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<PasswordHash>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure")
        };

        error.Code.Should().Be("PASSWORDHASH.Validation");
    }

    [Fact]
    public void Equality_WithSameHash_ShouldBeEqual()
    {
        const string hash = "$argon2id$v=19$m=65536,t=3,p=4$c29tZXNhbHQ$hash123";
        Result<PasswordHash> passwordHash1 = PasswordHashFactory.Create(hash);
        Result<PasswordHash> passwordHash2 = PasswordHashFactory.Create(hash);

        passwordHash1.Value.Should().Be(passwordHash2.Value);
    }

    [Fact]
    public void Equality_WithDifferentHash_ShouldNotBeEqual()
    {
        const string hash1 = "$argon2id$v=19$m=65536,t=3,p=4$c29tZXNhbHQ$hash123";
        const string hash2 = "$argon2id$v=19$m=65536,t=3,p=4$c29tZXNhbHQ$hash456";
        Result<PasswordHash> passwordHash1 = PasswordHashFactory.Create(hash1);
        Result<PasswordHash> passwordHash2 = PasswordHashFactory.Create(hash2);

        passwordHash1.Value.Should().NotBe(passwordHash2.Value);
    }

    [Fact]
    public void Create_WithMinimalValidHash_ShouldSucceed()
    {
        const string minimalHash = "a";

        Result<PasswordHash> result = PasswordHashFactory.Create(minimalHash);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(minimalHash);
    }

    [Fact]
    public void Create_WithLongHash_ShouldSucceed()
    {
        string longHash = new('x', 500);

        Result<PasswordHash> result = PasswordHashFactory.Create(longHash);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(longHash);
    }
}
