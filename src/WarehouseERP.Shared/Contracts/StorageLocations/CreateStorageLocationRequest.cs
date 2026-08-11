namespace WarehouseERP.Shared.Contracts.StorageLocations;

public sealed class CreateStorageLocationRequest
{
    public required Guid WarehouseId { get; init; }
    public required string Code { get; init; }
    public string? Description { get; init; }
}
