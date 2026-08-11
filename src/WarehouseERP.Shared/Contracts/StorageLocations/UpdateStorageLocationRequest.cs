namespace WarehouseERP.Shared.Contracts.StorageLocations;

public sealed class UpdateStorageLocationRequest
{
    public required string Code { get; init; }
    public string? Description { get; init; }
}
