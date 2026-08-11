using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Warehouses.StorageLocations;

public interface IStorageLocationRepository
{
    Task<StorageLocation?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<StorageLocation>> GetAllAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<StorageLocation>> GetByWarehouseIdAsync(Guid warehouseId, CancellationToken cancellationToken);

    // Implementations must match codes case-insensitively within the given warehouse.
    Task<StorageLocation?> GetByWarehouseIdAndCodeAsync(Guid warehouseId, string code, CancellationToken cancellationToken);

    Task AddAsync(StorageLocation storageLocation, CancellationToken cancellationToken);

    Task UpdateAsync(StorageLocation storageLocation, CancellationToken cancellationToken);
}
