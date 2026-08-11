namespace WarehouseERP.Shared.Contracts.Warehouses;

public sealed class WarehouseDto
{
    public required Guid Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? Address { get; init; }
    public required bool IsActive { get; init; }
}
