using Shared.Kernel;
using UserManagement.Domain.ValueObjects;

namespace Tests.UserManagement.Domain.ValueObjects;

public sealed class UserIdTests
{
    [Fact]
    public void Create_WhenCalledWithoutParameters_ShouldReturnUserIdWithGuidV7()
    {
        UserId userId = UserIdFactory.Create();

        userId.Should().NotBeNull();
        userId.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Create_WhenCalledWithGuid_ShouldReturnUserIdWithSpecifiedValue()
    {
        var expectedValue = Guid.NewGuid();

        UserId userId = UserIdFactory.Create(expectedValue);

        userId.Value.Should().Be(expectedValue);
        userId.Should().NotBeNull();
    }

    [Fact]
    public void Equality_WhenSameValue_ShouldBeEqual()
    {
        var value = Guid.NewGuid();
        UserId userId1 = UserIdFactory.Create(value);
        UserId userId2 = UserIdFactory.Create(value);

        userId1.Should().Be(userId2);
        (userId1 == userId2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WhenDifferentValue_ShouldNotBeEqual()
    {
        UserId userId1 = UserIdFactory.Create();
        UserId userId2 = UserIdFactory.Create();

        userId1.Should().NotBe(userId2);
        (userId1 != userId2).Should().BeTrue();
    }
}
