namespace WarehouseERP.Application.Warehouses.StorageLocations.Commands.CreateStorageLocation;

public sealed class CreateStorageLocationCommand
{
    public required Guid WarehouseId { get; init; }
    public required string Code { get; init; }
    public string? Description { get; init; }
}
