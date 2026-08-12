namespace WarehouseERP.Infrastructure.Identity;

// Mirrors WarehouseERP.Shared.Contracts.Auth.RoleNames. Infrastructure cannot reference Shared,
// so the four role names are duplicated here for RoleSeeder/IdentitySeeder to consume.
public static class Roles
{
    public const string Admin = "Admin";
    public const string Warehouse = "Warehouse";
    public const string Purchasing = "Purchasing";
    public const string Sales = "Sales";

    public static readonly IReadOnlyList<string> All = new[] { Admin, Warehouse, Purchasing, Sales };
}
