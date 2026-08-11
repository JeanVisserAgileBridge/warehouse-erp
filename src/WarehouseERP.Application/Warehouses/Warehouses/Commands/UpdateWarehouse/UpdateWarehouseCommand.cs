namespace WarehouseERP.Application.Warehouses.Warehouses.Commands.UpdateWarehouse;

public sealed class UpdateWarehouseCommand
{
    public required Guid Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? Address { get; init; }
}
