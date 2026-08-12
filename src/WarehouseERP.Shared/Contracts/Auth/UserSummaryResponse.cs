namespace WarehouseERP.Shared.Contracts.Auth;

public sealed class UserSummaryResponse
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required IReadOnlyList<string> Roles { get; init; }
}
