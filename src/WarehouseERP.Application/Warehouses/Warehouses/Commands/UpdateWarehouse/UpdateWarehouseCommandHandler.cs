using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Warehouses.Warehouses.Commands.UpdateWarehouse;

public sealed class UpdateWarehouseCommandHandler : ICommandHandler<UpdateWarehouseCommand, WarehouseDto>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public UpdateWarehouseCommandHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<WarehouseDto> HandleAsync(UpdateWarehouseCommand command, CancellationToken cancellationToken)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Warehouse with id '{command.Id}' was not found.");

        var warehouseWithSameCode = await _warehouseRepository.GetByCodeAsync(command.Code, cancellationToken);
        if (warehouseWithSameCode is not null && warehouseWithSameCode.Id != warehouse.Id)
        {
            throw new DuplicateCodeException($"A warehouse with code '{command.Code}' already exists.");
        }

        warehouse.ChangeCode(command.Code);
        warehouse.Rename(command.Name);
        warehouse.ChangeAddress(command.Address);

        await _warehouseRepository.UpdateAsync(warehouse, cancellationToken);

        return WarehouseDto.FromDomain(warehouse);
    }
}
