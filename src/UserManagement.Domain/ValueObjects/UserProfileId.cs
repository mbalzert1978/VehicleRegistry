using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.ValueObjects;

public sealed record UserProfileId(Guid Value) : StronglyTypedId<Guid>(Value);

public static class UserProfileIdFactory
{
    public static UserProfileId Create()
    {
        var value = Guid.CreateVersion7();

        Debug.Assert(value != Guid.Empty, "Generated Guid must not be empty");

        UserProfileId id = new(value);

        Debug.Assert(id.Value == value, "UserProfileId value must match input");
        Debug.Assert(
            id.Value != Guid.Empty,
            "UserProfileId value must not be empty after creation"
        );

        return id;
    }

    public static UserProfileId Create(Guid value)
    {
        Debug.Assert(value != Guid.Empty, "Guid value must not be empty");

        UserProfileId id = new(value);

        Debug.Assert(id.Value == value, "UserProfileId value must match input");
        Debug.Assert(
            id.Value != Guid.Empty,
            "UserProfileId value must not be empty after creation"
        );

        return id;
    }
}
