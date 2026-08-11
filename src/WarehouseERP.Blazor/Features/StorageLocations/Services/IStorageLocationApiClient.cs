using WarehouseERP.Shared.Contracts.StorageLocations;

namespace WarehouseERP.Blazor.Features.StorageLocations.Services;

public interface IStorageLocationApiClient
{
    Task<IReadOnlyList<StorageLocationDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<StorageLocationDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StorageLocationDto>> GetByWarehouseIdAsync(Guid warehouseId, CancellationToken cancellationToken = default);

    Task<StorageLocationDto> CreateAsync(CreateStorageLocationRequest request, CancellationToken cancellationToken = default);

    Task<StorageLocationDto> UpdateAsync(Guid id, UpdateStorageLocationRequest request, CancellationToken cancellationToken = default);

    Task<StorageLocationDto> ActivateAsync(Guid id, CancellationToken cancellationToken = default);

    Task<StorageLocationDto> DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
}
