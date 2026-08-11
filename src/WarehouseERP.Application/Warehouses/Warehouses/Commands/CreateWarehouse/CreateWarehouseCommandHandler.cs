using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Warehouses.Warehouses.Commands.CreateWarehouse;

public sealed class CreateWarehouseCommandHandler : ICommandHandler<CreateWarehouseCommand, WarehouseDto>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public CreateWarehouseCommandHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<WarehouseDto> HandleAsync(CreateWarehouseCommand command, CancellationToken cancellationToken)
    {
        var existingWarehouse = await _warehouseRepository.GetByCodeAsync(command.Code, cancellationToken);
        if (existingWarehouse is not null)
        {
            throw new DuplicateCodeException($"A warehouse with code '{command.Code}' already exists.");
        }

        var warehouse = Warehouse.Create(command.Code, command.Name, command.Address);

        await _warehouseRepository.AddAsync(warehouse, cancellationToken);

        return WarehouseDto.FromDomain(warehouse);
    }
}
