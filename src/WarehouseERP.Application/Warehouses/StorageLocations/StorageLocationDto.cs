using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Warehouses.StorageLocations;

public sealed class StorageLocationDto
{
    public required Guid Id { get; init; }
    public required Guid WarehouseId { get; init; }
    public required string Code { get; init; }
    public string? Description { get; init; }
    public required bool IsActive { get; init; }

    public static StorageLocationDto FromDomain(StorageLocation storageLocation)
    {
        return new StorageLocationDto
        {
            Id = storageLocation.Id,
            WarehouseId = storageLocation.WarehouseId,
            Code = storageLocation.Code,
            Description = storageLocation.Description,
            IsActive = storageLocation.IsActive
        };
    }
}
