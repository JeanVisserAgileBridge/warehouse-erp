using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocationsByWarehouseId;

public sealed class GetStorageLocationsByWarehouseIdQueryHandler : IQueryHandler<GetStorageLocationsByWarehouseIdQuery, IReadOnlyList<StorageLocationDto>>
{
    private readonly IStorageLocationRepository _storageLocationRepository;

    public GetStorageLocationsByWarehouseIdQueryHandler(IStorageLocationRepository storageLocationRepository)
    {
        _storageLocationRepository = storageLocationRepository;
    }

    public async Task<IReadOnlyList<StorageLocationDto>> HandleAsync(GetStorageLocationsByWarehouseIdQuery query, CancellationToken cancellationToken)
    {
        var storageLocations = await _storageLocationRepository.GetByWarehouseIdAsync(query.WarehouseId, cancellationToken);

        return storageLocations.Select(StorageLocationDto.FromDomain).ToList();
    }
}
