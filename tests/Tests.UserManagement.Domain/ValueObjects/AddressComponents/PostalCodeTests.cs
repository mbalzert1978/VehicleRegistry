using Shared.Kernel;
using UserManagement.Domain.ValueObjects.AddressComponents;

namespace Tests.UserManagement.Domain.ValueObjects.AddressComponents;

public sealed class PostalCodeTests
{
    [Fact]
    public void Create_WithValidPostalCode_ShouldSucceed()
    {
        const string value = "12345";

        Result<PostalCode> result = PostalCodeFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithEmptyPostalCode_ShouldFail()
    {
        const string value = "";

        Result<PostalCode> result = PostalCodeFactory.Create(value);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<PostalCode>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("POSTALCODE.Validation");
        error.Message.Should().Be("PostalCode cannot be empty");
    }

    [Fact]
    public void Create_WithWhitespacePostalCode_ShouldFail()
    {
        const string value = "   ";

        Result<PostalCode> result = PostalCodeFactory.Create(value);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<PostalCode>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("POSTALCODE.Validation");
        error.Message.Should().Be("PostalCode cannot be empty");
    }

    [Fact]
    public void Create_WithTooLongPostalCode_ShouldFail()
    {
        string value = new('1', 21);

        Result<PostalCode> result = PostalCodeFactory.Create(value);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<PostalCode>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("POSTALCODE.Validation");
        error.Message.Should().Be("PostalCode cannot exceed maximum length of 20 characters");
    }

    [Fact]
    public void Create_WithMaxLengthPostalCode_ShouldSucceed()
    {
        string value = new('1', 20);

        Result<PostalCode> result = PostalCodeFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().HaveLength(20);
    }

    [Fact]
    public void RecordEquality_WithSameValues_ShouldBeEqual()
    {
        const string value = "12345";

        Result<PostalCode> result1 = PostalCodeFactory.Create(value);
        Result<PostalCode> result2 = PostalCodeFactory.Create(value);

        result1.Value.Should().Be(result2.Value);
    }

    [Fact]
    public void RecordEquality_WithDifferentValues_ShouldNotBeEqual()
    {
        const string value1 = "12345";
        const string value2 = "67890";

        Result<PostalCode> result1 = PostalCodeFactory.Create(value1);
        Result<PostalCode> result2 = PostalCodeFactory.Create(value2);

        result1.Value.Should().NotBe(result2.Value);
    }

    [Fact]
    public void Immutability_ValueShouldBeReadOnly()
    {
        const string value = "12345";

        Result<PostalCode> result = PostalCodeFactory.Create(value);

        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithGermanPostalCode_ShouldSucceed()
    {
        const string value = "10115";

        Result<PostalCode> result = PostalCodeFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithUKPostalCode_ShouldSucceed()
    {
        const string value = "SW1A 1AA";

        Result<PostalCode> result = PostalCodeFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithCanadianPostalCode_ShouldSucceed()
    {
        const string value = "K1A 0B1";

        Result<PostalCode> result = PostalCodeFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }
}
