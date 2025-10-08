using AwesomeAssertions;
using UserManagement.Domain.ValueObjects;

namespace Tests.UserManagement.Domain.ValueObjects;

public sealed class AddressIdTests
{
    [Fact]
    public void AddressId_WhenCreatedWithoutParameter_ShouldHaveNonEmptyValue()
    {
        AddressId addressId = AddressIdFactory.Create();

        addressId.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void AddressId_WhenCreatedWithValue_ShouldReturnSameValue()
    {
        var guid = Guid.CreateVersion7();

        AddressId addressId = AddressIdFactory.Create(guid);

        addressId.Value.Should().Be(guid);
        addressId.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void AddressId_WhenComparedWithSameValue_ShouldBeEqual()
    {
        var guid = Guid.CreateVersion7();
        AddressId addressId1 = AddressIdFactory.Create(guid);
        AddressId addressId2 = AddressIdFactory.Create(guid);

        addressId1.Should().Be(addressId2);
    }

    [Fact]
    public void AddressId_WhenComparedWithDifferentValue_ShouldNotBeEqual()
    {
        AddressId addressId1 = AddressIdFactory.Create();
        AddressId addressId2 = AddressIdFactory.Create();

        addressId1.Should().NotBe(addressId2);
    }
}
