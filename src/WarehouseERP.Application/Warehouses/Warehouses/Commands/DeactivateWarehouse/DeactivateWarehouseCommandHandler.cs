using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Warehouses.Warehouses.Commands.DeactivateWarehouse;

public sealed class DeactivateWarehouseCommandHandler : ICommandHandler<DeactivateWarehouseCommand, WarehouseDto>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public DeactivateWarehouseCommandHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<WarehouseDto> HandleAsync(DeactivateWarehouseCommand command, CancellationToken cancellationToken)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Warehouse with id '{command.Id}' was not found.");

        warehouse.Deactivate();

        await _warehouseRepository.UpdateAsync(warehouse, cancellationToken);

        return WarehouseDto.FromDomain(warehouse);
    }
}
