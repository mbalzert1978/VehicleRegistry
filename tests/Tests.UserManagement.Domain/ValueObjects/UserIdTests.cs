using Shared.Kernel;
using UserManagement.Domain.ValueObjects;

namespace Tests.UserManagement.Domain.ValueObjects;

public sealed class UserIdTests
{
    [Fact]
    public void Create_WhenCalled_ShouldReturnUserId()
    {
        var expectedValue = Guid.NewGuid();

        UserId userId = new(expectedValue);

        userId.Value.Should().Be(expectedValue);
        userId.Should().NotBeNull();
    }

    [Fact]
    public void Equality_WhenSameValue_ShouldBeEqual()
    {
        var value = Guid.NewGuid();
        UserId userId1 = new(value);
        UserId userId2 = new(value);

        userId1.Should().Be(userId2);
        (userId1 == userId2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WhenDifferentValue_ShouldNotBeEqual()
    {
        UserId userId1 = new(Guid.NewGuid());
        UserId userId2 = new(Guid.NewGuid());

        userId1.Should().NotBe(userId2);
        (userId1 != userId2).Should().BeTrue();
    }
}
