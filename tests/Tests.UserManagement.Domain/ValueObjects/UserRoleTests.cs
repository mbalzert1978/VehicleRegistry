using AwesomeAssertions;
using UserManagement.Domain.ValueObjects;

namespace Tests.UserManagement.Domain.ValueObjects;

public sealed class UserRoleTests
{
    [Fact]
    public void CreateStandardUser_ShouldReturnStandardUser()
    {
        UserRole role = UserRoleFactory.CreateStandardUser();

        role.Should().BeOfType<StandardUser>();
        role.RoleName.Should().Be("StandardUser");
    }

    [Fact]
    public void CreateAdmin_ShouldReturnAdmin()
    {
        UserRole role = UserRoleFactory.CreateAdmin();

        role.Should().BeOfType<Admin>();
        role.RoleName.Should().Be("Admin");
    }

    [Fact]
    public void StandardUser_ShouldHaveCorrectRoleName()
    {
        UserRole role = new StandardUser();

        role.RoleName.Should().Be("StandardUser");
    }

    [Fact]
    public void Admin_ShouldHaveCorrectRoleName()
    {
        UserRole role = new Admin();

        role.RoleName.Should().Be("Admin");
    }

    [Fact]
    public void StandardUser_AndAdmin_ShouldNotBeEqual()
    {
        UserRole standardUser = new StandardUser();
        UserRole admin = new Admin();

        standardUser.Should().NotBe(admin);
    }

    [Fact]
    public void Equality_WithSameStandardUser_ShouldBeEqual()
    {
        UserRole role1 = new StandardUser();
        UserRole role2 = new StandardUser();

        role1.Should().Be(role2);
        (role1 == role2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WithSameAdmin_ShouldBeEqual()
    {
        UserRole role1 = new Admin();
        UserRole role2 = new Admin();

        role1.Should().Be(role2);
        (role1 == role2).Should().BeTrue();
    }

    [Fact]
    public void PatternMatching_WithStandardUser_ShouldWork()
    {
        UserRole role = new StandardUser();

        string result = role switch
        {
            StandardUser => "StandardUser",
            Admin => "Admin",
            _ => throw new InvalidOperationException("Unknown role")
        };

        result.Should().Be("StandardUser");
    }

    [Fact]
    public void PatternMatching_WithAdmin_ShouldWork()
    {
        UserRole role = new Admin();

        string result = role switch
        {
            StandardUser => "StandardUser",
            Admin => "Admin",
            _ => throw new InvalidOperationException("Unknown role")
        };

        result.Should().Be("Admin");
    }

    [Fact]
    public void StandardUser_ShouldBeImmutable()
    {
        UserRole role = new StandardUser();
        string originalRoleName = role.RoleName;

        string retrievedRoleName = role.RoleName;

        retrievedRoleName.Should().Be(originalRoleName);
        role.RoleName.Should().Be("StandardUser");
    }

    [Fact]
    public void Admin_ShouldBeImmutable()
    {
        UserRole role = new Admin();
        string originalRoleName = role.RoleName;

        string retrievedRoleName = role.RoleName;

        retrievedRoleName.Should().Be(originalRoleName);
        role.RoleName.Should().Be("Admin");
    }
}
