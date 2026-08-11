using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocations;

public sealed class GetStorageLocationsQueryHandler : IQueryHandler<GetStorageLocationsQuery, IReadOnlyList<StorageLocationDto>>
{
    private readonly IStorageLocationRepository _storageLocationRepository;

    public GetStorageLocationsQueryHandler(IStorageLocationRepository storageLocationRepository)
    {
        _storageLocationRepository = storageLocationRepository;
    }

    public async Task<IReadOnlyList<StorageLocationDto>> HandleAsync(GetStorageLocationsQuery query, CancellationToken cancellationToken)
    {
        var storageLocations = await _storageLocationRepository.GetAllAsync(cancellationToken);

        return storageLocations.Select(StorageLocationDto.FromDomain).ToList();
    }
}
