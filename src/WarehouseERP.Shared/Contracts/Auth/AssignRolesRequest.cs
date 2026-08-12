namespace WarehouseERP.Shared.Contracts.Auth;

public sealed class AssignRolesRequest
{
    public required IReadOnlyList<string> Roles { get; init; }
}
