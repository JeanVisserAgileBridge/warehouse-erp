namespace WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocationsByWarehouseId;

public sealed class GetStorageLocationsByWarehouseIdQuery
{
    public required Guid WarehouseId { get; init; }
}
