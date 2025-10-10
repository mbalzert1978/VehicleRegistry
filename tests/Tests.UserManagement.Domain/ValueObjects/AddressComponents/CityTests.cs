using Shared.Kernel;
using UserManagement.Domain.ValueObjects.AddressComponents;

namespace Tests.UserManagement.Domain.ValueObjects.AddressComponents;

public sealed class CityTests
{
    [Fact]
    public void Create_WithValidCity_ShouldSucceed()
    {
        const string value = "New York";

        Result<City> result = CityFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithEmptyCity_ShouldFail()
    {
        const string value = "";

        Result<City> result = CityFactory.Create(value);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<City>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("CITY.Validation");
        error.Message.Should().Be("City cannot be empty");
    }

    [Fact]
    public void Create_WithWhitespaceCity_ShouldFail()
    {
        const string value = "   ";

        Result<City> result = CityFactory.Create(value);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<City>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("CITY.Validation");
        error.Message.Should().Be("City cannot be empty");
    }

    [Fact]
    public void Create_WithTooLongCity_ShouldFail()
    {
        string value = new('a', 101);

        Result<City> result = CityFactory.Create(value);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<City>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("CITY.Validation");
        error.Message.Should().Be("City cannot exceed maximum length of 100 characters");
    }

    [Fact]
    public void Create_WithMaxLengthCity_ShouldSucceed()
    {
        string value = new('a', 100);

        Result<City> result = CityFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().HaveLength(100);
    }

    [Fact]
    public void RecordEquality_WithSameValues_ShouldBeEqual()
    {
        const string value = "New York";

        Result<City> result1 = CityFactory.Create(value);
        Result<City> result2 = CityFactory.Create(value);

        result1.Value.Should().Be(result2.Value);
    }

    [Fact]
    public void RecordEquality_WithDifferentValues_ShouldNotBeEqual()
    {
        const string value1 = "New York";
        const string value2 = "Los Angeles";

        Result<City> result1 = CityFactory.Create(value1);
        Result<City> result2 = CityFactory.Create(value2);

        result1.Value.Should().NotBe(result2.Value);
    }

    [Fact]
    public void Immutability_ValueShouldBeReadOnly()
    {
        const string value = "New York";

        Result<City> result = CityFactory.Create(value);

        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithSpecialCharacters_ShouldSucceed()
    {
        const string value = "São Paulo";

        Result<City> result = CityFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithHyphens_ShouldSucceed()
    {
        const string value = "Baden-Baden";

        Result<City> result = CityFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }
}
