using Shared.Kernel;
using UserManagement.Domain.ValueObjects.AddressComponents;

namespace Tests.UserManagement.Domain.ValueObjects.AddressComponents;

public sealed class StreetTests
{
    [Fact]
    public void Create_WithValidStreet_ShouldSucceed()
    {
        const string value = "Main Street 123";

        Result<Street> result = StreetFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithEmptyStreet_ShouldFail()
    {
        const string value = "";

        Result<Street> result = StreetFactory.Create(value);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<Street>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("STREET.Validation");
        error.Message.Should().Be("Street cannot be empty");
    }

    [Fact]
    public void Create_WithWhitespaceStreet_ShouldFail()
    {
        const string value = "   ";

        Result<Street> result = StreetFactory.Create(value);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<Street>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("STREET.Validation");
        error.Message.Should().Be("Street cannot be empty");
    }

    [Fact]
    public void Create_WithTooLongStreet_ShouldFail()
    {
        string value = new('a', 201);

        Result<Street> result = StreetFactory.Create(value);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<Street>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("STREET.Validation");
        error.Message.Should().Be("Street cannot exceed maximum length of 200 characters");
    }

    [Fact]
    public void Create_WithMaxLengthStreet_ShouldSucceed()
    {
        string value = new('a', 200);

        Result<Street> result = StreetFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().HaveLength(200);
    }

    [Fact]
    public void RecordEquality_WithSameValues_ShouldBeEqual()
    {
        const string value = "Main Street 123";

        Result<Street> result1 = StreetFactory.Create(value);
        Result<Street> result2 = StreetFactory.Create(value);

        result1.Value.Should().Be(result2.Value);
    }

    [Fact]
    public void RecordEquality_WithDifferentValues_ShouldNotBeEqual()
    {
        const string value1 = "Main Street 123";
        const string value2 = "Oak Avenue 456";

        Result<Street> result1 = StreetFactory.Create(value1);
        Result<Street> result2 = StreetFactory.Create(value2);

        result1.Value.Should().NotBe(result2.Value);
    }

    [Fact]
    public void Immutability_ValueShouldBeReadOnly()
    {
        const string value = "Main Street 123";

        Result<Street> result = StreetFactory.Create(value);

        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithSpecialCharacters_ShouldSucceed()
    {
        const string value = "Rue de l'Église 42";

        Result<Street> result = StreetFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithNumbers_ShouldSucceed()
    {
        const string value = "123 Main St.";

        Result<Street> result = StreetFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }
}
