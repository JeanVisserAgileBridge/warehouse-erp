using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Warehouses.Warehouses.Commands.ActivateWarehouse;

public sealed class ActivateWarehouseCommandHandler : ICommandHandler<ActivateWarehouseCommand, WarehouseDto>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public ActivateWarehouseCommandHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<WarehouseDto> HandleAsync(ActivateWarehouseCommand command, CancellationToken cancellationToken)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Warehouse with id '{command.Id}' was not found.");

        warehouse.Activate();

        await _warehouseRepository.UpdateAsync(warehouse, cancellationToken);

        return WarehouseDto.FromDomain(warehouse);
    }
}
