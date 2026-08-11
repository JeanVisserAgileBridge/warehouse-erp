using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocationById;

public sealed class GetStorageLocationByIdQueryHandler : IQueryHandler<GetStorageLocationByIdQuery, StorageLocationDto>
{
    private readonly IStorageLocationRepository _storageLocationRepository;

    public GetStorageLocationByIdQueryHandler(IStorageLocationRepository storageLocationRepository)
    {
        _storageLocationRepository = storageLocationRepository;
    }

    public async Task<StorageLocationDto> HandleAsync(GetStorageLocationByIdQuery query, CancellationToken cancellationToken)
    {
        var storageLocation = await _storageLocationRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException($"Storage location with id '{query.Id}' was not found.");

        return StorageLocationDto.FromDomain(storageLocation);
    }
}
