using AwesomeAssertions;
using UserManagement.Domain.ValueObjects;

namespace Tests.UserManagement.Domain.ValueObjects;


public sealed class UserProfileIdTests
{
    [Fact]
    public void UserProfileId_WhenCreatedWithoutParameter_ShouldHaveNonEmptyValue()
    {
        UserProfileId userId = UserProfileIdFactory.Create();

        userId.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void UserProfileId_WhenCreatedWithValue_ShouldReturnSameValue()
    {
        var guid = Guid.CreateVersion7();

        UserProfileId userId = UserProfileIdFactory.Create(guid);

        userId.Value.Should().Be(guid);
        userId.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void UserProfileId_WhenComparedWithSameValue_ShouldBeEqual()
    {
        var guid = Guid.CreateVersion7();
        UserProfileId userId1 = UserProfileIdFactory.Create(guid);
        UserProfileId userId2 = UserProfileIdFactory.Create(guid);

        userId1.Should().Be(userId2);
    }

    [Fact]
    public void UserProfileId_WhenComparedWithDifferentValue_ShouldNotBeEqual()
    {
        UserProfileId userId1 = UserProfileIdFactory.Create();
        UserProfileId userId2 = UserProfileIdFactory.Create();

        userId1.Should().NotBe(userId2);
    }
}
