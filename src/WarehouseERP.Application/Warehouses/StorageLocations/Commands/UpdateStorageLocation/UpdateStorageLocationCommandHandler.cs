using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Warehouses.StorageLocations.Commands.UpdateStorageLocation;

public sealed class UpdateStorageLocationCommandHandler : ICommandHandler<UpdateStorageLocationCommand, StorageLocationDto>
{
    private readonly IStorageLocationRepository _storageLocationRepository;

    public UpdateStorageLocationCommandHandler(IStorageLocationRepository storageLocationRepository)
    {
        _storageLocationRepository = storageLocationRepository;
    }

    public async Task<StorageLocationDto> HandleAsync(UpdateStorageLocationCommand command, CancellationToken cancellationToken)
    {
        var storageLocation = await _storageLocationRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Storage location with id '{command.Id}' was not found.");

        var storageLocationWithSameCode = await _storageLocationRepository.GetByWarehouseIdAndCodeAsync(
            storageLocation.WarehouseId, command.Code, cancellationToken);
        if (storageLocationWithSameCode is not null && storageLocationWithSameCode.Id != storageLocation.Id)
        {
            throw new DuplicateCodeException($"A storage location with code '{command.Code}' already exists in this warehouse.");
        }

        storageLocation.ChangeCode(command.Code);
        storageLocation.ChangeDescription(command.Description);

        await _storageLocationRepository.UpdateAsync(storageLocation, cancellationToken);

        return StorageLocationDto.FromDomain(storageLocation);
    }
}
