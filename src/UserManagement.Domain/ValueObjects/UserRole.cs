using System.Diagnostics;

namespace UserManagement.Domain.ValueObjects;

/// <summary>
/// Represents a user role as a discriminated union.
/// Variants: StandardUser, Admin
/// </summary>
public abstract record UserRole
{
    /// <summary>
    /// Gets the role name for claims/JWT.
    /// </summary>
    public abstract string RoleName { get; }
}

/// <summary>
/// Standard user role with basic permissions.
/// </summary>
public sealed record StandardUser : UserRole
{
    public override string RoleName => nameof(StandardUser);
}

/// <summary>
/// Administrator role with elevated permissions.
/// </summary>
public sealed record Admin : UserRole
{
    public override string RoleName => nameof(Admin);
}

/// <summary>
/// Factory for creating UserRole instances.
/// </summary>
public static class UserRoleFactory
{
    /// <summary>
    /// Creates a StandardUser role.
    /// </summary>
    public static UserRole CreateStandardUser()
    {
        UserRole role = new StandardUser();

        Debug.Assert(role is StandardUser, "Role should be StandardUser");
        Debug.Assert(role.RoleName == nameof(StandardUser), "RoleName should match type name");

        return role;
    }

    /// <summary>
    /// Creates an Admin role.
    /// </summary>
    public static UserRole CreateAdmin()
    {
        UserRole role = new Admin();

        Debug.Assert(role is Admin, "Role should be Admin");
        Debug.Assert(role.RoleName == nameof(Admin), "RoleName should match type name");

        return role;
    }
}
