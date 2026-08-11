namespace WarehouseERP.Application.Warehouses.StorageLocations.Commands.UpdateStorageLocation;

public sealed class UpdateStorageLocationCommand
{
    public required Guid Id { get; init; }
    public required string Code { get; init; }
    public string? Description { get; init; }
}
