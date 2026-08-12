namespace WarehouseERP.Shared.Contracts.Auth;

// Mirrors WarehouseERP.Infrastructure.Identity.Roles. Shared cannot reference Infrastructure,
// so the four role names are duplicated here for Blazor (AuthorizeView/[Authorize(Roles=...)])
// and the Api authorization policies to consume without an architecture-rule violation.
public static class RoleNames
{
    public const string Admin = "Admin";
    public const string Warehouse = "Warehouse";
    public const string Purchasing = "Purchasing";
    public const string Sales = "Sales";

    public static readonly IReadOnlyList<string> All = new[] { Admin, Warehouse, Purchasing, Sales };
}
