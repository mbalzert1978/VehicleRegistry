using Shared.Kernel;
using UserManagement.Domain.ValueObjects;

namespace Tests.UserManagement.Domain.ValueObjects;

public sealed class NameTests
{
    [Fact]
    public void Create_WithValidFirstAndLastName_ShouldSucceed()
    {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";

        // Act
        Result<Name> result = NameFactory.Create(firstName, lastName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.FirstName.Should().Be(firstName);
        result.Value.LastName.Should().Be(lastName);
    }

    [Fact]
    public void Create_WithEmptyFirstName_ShouldFail()
    {
        // Arrange
        const string firstName = "";
        const string lastName = "Doe";

        // Act
        Result<Name> result = NameFactory.Create(firstName, lastName);

        // Assert
        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<Name>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure")
        };

        error.Code.Should().Be("FIRSTNAME.Validation");
        error.Message.Should().Be("FirstName cannot be empty");
    }

    [Fact]
    public void Create_WithEmptyLastName_ShouldFail()
    {
        Result<Name> result = NameFactory.Create("John", "");

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<Name>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure")
        };

        error.Code.Should().Be("LASTNAME.Validation");
        error.Message.Should().Be("LastName cannot be empty");
    }

    [Fact]
    public void Create_WithWhitespaceFirstName_ShouldFail()
    {
        Result<Name> result = NameFactory.Create("   ", "Doe");

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<Name>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure")
        };

        error.Code.Should().Be("FIRSTNAME.Validation");
        error.Message.Should().Be("FirstName cannot be empty");
    }

    [Fact]
    public void Create_WithWhitespaceLastName_ShouldFail()
    {
        Result<Name> result = NameFactory.Create("John", "   ");

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<Name>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure")
        };

        error.Code.Should().Be("LASTNAME.Validation");
        error.Message.Should().Be("LastName cannot be empty");
    }

    [Fact]
    public void Create_WithTooLongFirstName_ShouldFail()
    {
        string firstName = new('A', 101);

        Result<Name> result = NameFactory.Create(firstName, "Doe");

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<Name>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure")
        };

        error.Code.Should().Be("FIRSTNAME.Validation");
        error.Message.Should().Be("FirstName cannot exceed maximum length of 100 characters");
    }

    [Fact]
    public void Create_WithTooLongLastName_ShouldFail()
    {
        string lastName = new('A', 101);

        Result<Name> result = NameFactory.Create("John", lastName);

        result.IsFailure.Should().BeTrue();

        Error error = result switch
        {
            Failure<Name>(var e) => e,
            _ => throw new InvalidOperationException("Expected Failure")
        };

        error.Code.Should().Be("LASTNAME.Validation");
        error.Message.Should().Be("LastName cannot exceed maximum length of 100 characters");
    }

    [Fact]
    public void Create_WithMaxLengthNames_ShouldSucceed()
    {
        // Arrange
        string firstName = new('A', 100); // Max 100 characters
        string lastName = new('B', 100);

        // Act
        Result<Name> result = NameFactory.Create(firstName, lastName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.FirstName.Should().Be(firstName);
        result.Value.LastName.Should().Be(lastName);
    }

    [Fact]
    public void Equality_WithSameValues_ShouldBeEqual()
    {
        // Arrange
        Result<Name> name1 = NameFactory.Create("John", "Doe");
        Result<Name> name2 = NameFactory.Create("John", "Doe");

        // Assert
        name1.Value.Should().Be(name2.Value);
    }

    [Fact]
    public void Equality_WithDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        Result<Name> name1 = NameFactory.Create("John", "Doe");
        Result<Name> name2 = NameFactory.Create("Jane", "Doe");

        // Assert
        name1.Value.Should().NotBe(name2.Value);
    }
}
