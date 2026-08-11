namespace WarehouseERP.Shared.Contracts.Warehouses;

public sealed class UpdateWarehouseRequest
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? Address { get; init; }
}
