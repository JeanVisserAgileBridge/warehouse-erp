namespace WarehouseERP.Shared.Contracts.Auth;

public sealed class CreateUserRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public IReadOnlyList<string> Roles { get; init; } = Array.Empty<string>();
}
