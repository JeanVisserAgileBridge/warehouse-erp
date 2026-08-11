using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Warehouses.StorageLocations.Commands.ActivateStorageLocation;

public sealed class ActivateStorageLocationCommandHandler : ICommandHandler<ActivateStorageLocationCommand, StorageLocationDto>
{
    private readonly IStorageLocationRepository _storageLocationRepository;

    public ActivateStorageLocationCommandHandler(IStorageLocationRepository storageLocationRepository)
    {
        _storageLocationRepository = storageLocationRepository;
    }

    public async Task<StorageLocationDto> HandleAsync(ActivateStorageLocationCommand command, CancellationToken cancellationToken)
    {
        var storageLocation = await _storageLocationRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Storage location with id '{command.Id}' was not found.");

        storageLocation.Activate();

        await _storageLocationRepository.UpdateAsync(storageLocation, cancellationToken);

        return StorageLocationDto.FromDomain(storageLocation);
    }
}
