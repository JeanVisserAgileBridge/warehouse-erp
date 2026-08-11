using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Warehouses.Warehouses;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Warehouses.StorageLocations.Commands.CreateStorageLocation;

public sealed class CreateStorageLocationCommandHandler : ICommandHandler<CreateStorageLocationCommand, StorageLocationDto>
{
    private readonly IStorageLocationRepository _storageLocationRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    public CreateStorageLocationCommandHandler(
        IStorageLocationRepository storageLocationRepository,
        IWarehouseRepository warehouseRepository)
    {
        _storageLocationRepository = storageLocationRepository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task<StorageLocationDto> HandleAsync(CreateStorageLocationCommand command, CancellationToken cancellationToken)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(command.WarehouseId, cancellationToken)
            ?? throw new NotFoundException($"Warehouse with id '{command.WarehouseId}' was not found.");

        if (!warehouse.IsActive)
        {
            throw new InactiveWarehouseException($"Warehouse with id '{command.WarehouseId}' is not active.");
        }

        var existingStorageLocation = await _storageLocationRepository.GetByWarehouseIdAndCodeAsync(
            command.WarehouseId, command.Code, cancellationToken);
        if (existingStorageLocation is not null)
        {
            throw new DuplicateCodeException($"A storage location with code '{command.Code}' already exists in this warehouse.");
        }

        var storageLocation = StorageLocation.Create(command.WarehouseId, command.Code, command.Description);

        await _storageLocationRepository.AddAsync(storageLocation, cancellationToken);

        return StorageLocationDto.FromDomain(storageLocation);
    }
}
