using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Warehouses.StorageLocations.Commands.DeactivateStorageLocation;

public sealed class DeactivateStorageLocationCommandHandler : ICommandHandler<DeactivateStorageLocationCommand, StorageLocationDto>
{
    private readonly IStorageLocationRepository _storageLocationRepository;

    public DeactivateStorageLocationCommandHandler(IStorageLocationRepository storageLocationRepository)
    {
        _storageLocationRepository = storageLocationRepository;
    }

    public async Task<StorageLocationDto> HandleAsync(DeactivateStorageLocationCommand command, CancellationToken cancellationToken)
    {
        var storageLocation = await _storageLocationRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Storage location with id '{command.Id}' was not found.");

        storageLocation.Deactivate();

        await _storageLocationRepository.UpdateAsync(storageLocation, cancellationToken);

        return StorageLocationDto.FromDomain(storageLocation);
    }
}
