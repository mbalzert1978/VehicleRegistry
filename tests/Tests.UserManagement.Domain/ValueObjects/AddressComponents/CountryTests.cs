using Shared.Kernel;
using UserManagement.Domain.ValueObjects.AddressComponents;

namespace Tests.UserManagement.Domain.ValueObjects.AddressComponents;

public sealed class CountryTests
{
    [Fact]
    public void Create_WithValidCountry_ShouldSucceed()
    {
        const string value = "United States";

        Result<Country> result = CountryFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithEmptyCountry_ShouldFail()
    {
        const string value = "";

        Result<Country> result = CountryFactory.Create(value);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<Country>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("COUNTRY.Validation");
        error.Message.Should().Be("Country cannot be empty");
    }

    [Fact]
    public void Create_WithWhitespaceCountry_ShouldFail()
    {
        const string value = "   ";

        Result<Country> result = CountryFactory.Create(value);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<Country>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("COUNTRY.Validation");
        error.Message.Should().Be("Country cannot be empty");
    }

    [Fact]
    public void Create_WithTooLongCountry_ShouldFail()
    {
        string value = new('a', 101);

        Result<Country> result = CountryFactory.Create(value);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<Country>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure"),
        };

        error.Code.Should().Be("COUNTRY.Validation");
        error.Message.Should().Be("Country cannot exceed maximum length of 100 characters");
    }

    [Fact]
    public void Create_WithMaxLengthCountry_ShouldSucceed()
    {
        string value = new('a', 100);

        Result<Country> result = CountryFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().HaveLength(100);
    }

    [Fact]
    public void RecordEquality_WithSameValues_ShouldBeEqual()
    {
        const string value = "United States";

        Result<Country> result1 = CountryFactory.Create(value);
        Result<Country> result2 = CountryFactory.Create(value);

        result1.Value.Should().Be(result2.Value);
    }

    [Fact]
    public void RecordEquality_WithDifferentValues_ShouldNotBeEqual()
    {
        const string value1 = "United States";
        const string value2 = "Germany";

        Result<Country> result1 = CountryFactory.Create(value1);
        Result<Country> result2 = CountryFactory.Create(value2);

        result1.Value.Should().NotBe(result2.Value);
    }

    [Fact]
    public void Immutability_ValueShouldBeReadOnly()
    {
        const string value = "United States";

        Result<Country> result = CountryFactory.Create(value);

        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithGermany_ShouldSucceed()
    {
        const string value = "Germany";

        Result<Country> result = CountryFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithSpecialCharacters_ShouldSucceed()
    {
        const string value = "Côte d'Ivoire";

        Result<Country> result = CountryFactory.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }
}
