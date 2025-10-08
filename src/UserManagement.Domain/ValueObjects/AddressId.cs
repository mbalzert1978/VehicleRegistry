using System.Diagnostics;
using Shared.Kernel;

namespace UserManagement.Domain.ValueObjects;

public sealed record AddressId(Guid Value) : StronglyTypedId<Guid>(Value);

public static class AddressIdFactory
{
    public static AddressId Create()
    {
        var value = Guid.CreateVersion7();

        Debug.Assert(value != Guid.Empty, "Generated Guid must not be empty");

        var id = new AddressId(value);

        Debug.Assert(id.Value == value, "AddressId value must match input");
        Debug.Assert(id.Value != Guid.Empty, "AddressId value must not be empty after creation");

        return id;
    }

    public static AddressId Create(Guid value)
    {
        Debug.Assert(value != Guid.Empty, "Guid value must not be empty");

        var id = new AddressId(value);

        Debug.Assert(id.Value == value, "AddressId value must match input");
        Debug.Assert(id.Value != Guid.Empty, "AddressId value must not be empty after creation");

        return id;
    }
}
