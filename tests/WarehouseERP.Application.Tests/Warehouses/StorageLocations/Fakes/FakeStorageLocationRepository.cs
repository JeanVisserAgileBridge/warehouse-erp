using WarehouseERP.Application.Warehouses.StorageLocations;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.StorageLocations.Fakes;

public sealed class FakeStorageLocationRepository : IStorageLocationRepository
{
    private readonly List<StorageLocation> _storageLocations = new();

    public CancellationToken? LastCancellationToken { get; private set; }

    public void Seed(StorageLocation storageLocation)
    {
        _storageLocations.Add(storageLocation);
    }

    public Task<StorageLocation?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_storageLocations.FirstOrDefault(s => s.Id == id));
    }

    public Task<IReadOnlyList<StorageLocation>> GetAllAsync(CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<StorageLocation>>(_storageLocations.ToList());
    }

    public Task<IReadOnlyList<StorageLocation>> GetByWarehouseIdAsync(Guid warehouseId, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<StorageLocation>>(
            _storageLocations.Where(s => s.WarehouseId == warehouseId).ToList());
    }

    public Task<StorageLocation?> GetByWarehouseIdAndCodeAsync(Guid warehouseId, string code, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_storageLocations.FirstOrDefault(
            s => s.WarehouseId == warehouseId && string.Equals(s.Code, code, StringComparison.OrdinalIgnoreCase)));
    }

    public Task AddAsync(StorageLocation storageLocation, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        _storageLocations.Add(storageLocation);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(StorageLocation storageLocation, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.CompletedTask;
    }
}
