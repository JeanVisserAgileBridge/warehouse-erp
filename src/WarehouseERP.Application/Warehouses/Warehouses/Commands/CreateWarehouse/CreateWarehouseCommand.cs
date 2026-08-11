namespace WarehouseERP.Application.Warehouses.Warehouses.Commands.CreateWarehouse;

public sealed class CreateWarehouseCommand
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? Address { get; init; }
}
