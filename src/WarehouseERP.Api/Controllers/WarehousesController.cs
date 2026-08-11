using Microsoft.AspNetCore.Mvc;
using WarehouseERP.Api.Contracts.Warehouses;
using WarehouseERP.Application.Common;
using WarehouseERP.Application.Warehouses.Warehouses.Commands.ActivateWarehouse;
using WarehouseERP.Application.Warehouses.Warehouses.Commands.CreateWarehouse;
using WarehouseERP.Application.Warehouses.Warehouses.Commands.DeactivateWarehouse;
using WarehouseERP.Application.Warehouses.Warehouses.Commands.UpdateWarehouse;
using WarehouseERP.Application.Warehouses.Warehouses.Queries.GetWarehouseById;
using WarehouseERP.Application.Warehouses.Warehouses.Queries.GetWarehouses;
using WarehouseERP.Shared.Contracts.Warehouses;
using ApplicationWarehouseDto = WarehouseERP.Application.Warehouses.Warehouses.WarehouseDto;

namespace WarehouseERP.Api.Controllers;

[ApiController]
[Route("api/warehouses")]
public sealed class WarehousesController : ControllerBase
{
    private readonly IQueryHandler<GetWarehousesQuery, IReadOnlyList<ApplicationWarehouseDto>> _getWarehouses;
    private readonly IQueryHandler<GetWarehouseByIdQuery, ApplicationWarehouseDto> _getWarehouseById;
    private readonly ICommandHandler<CreateWarehouseCommand, ApplicationWarehouseDto> _createWarehouse;
    private readonly ICommandHandler<UpdateWarehouseCommand, ApplicationWarehouseDto> _updateWarehouse;
    private readonly ICommandHandler<ActivateWarehouseCommand, ApplicationWarehouseDto> _activateWarehouse;
    private readonly ICommandHandler<DeactivateWarehouseCommand, ApplicationWarehouseDto> _deactivateWarehouse;

    public WarehousesController(
        IQueryHandler<GetWarehousesQuery, IReadOnlyList<ApplicationWarehouseDto>> getWarehouses,
        IQueryHandler<GetWarehouseByIdQuery, ApplicationWarehouseDto> getWarehouseById,
        ICommandHandler<CreateWarehouseCommand, ApplicationWarehouseDto> createWarehouse,
        ICommandHandler<UpdateWarehouseCommand, ApplicationWarehouseDto> updateWarehouse,
        ICommandHandler<ActivateWarehouseCommand, ApplicationWarehouseDto> activateWarehouse,
        ICommandHandler<DeactivateWarehouseCommand, ApplicationWarehouseDto> deactivateWarehouse)
    {
        _getWarehouses = getWarehouses;
        _getWarehouseById = getWarehouseById;
        _createWarehouse = createWarehouse;
        _updateWarehouse = updateWarehouse;
        _activateWarehouse = activateWarehouse;
        _deactivateWarehouse = deactivateWarehouse;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<WarehouseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var warehouses = await _getWarehouses.HandleAsync(new GetWarehousesQuery(), cancellationToken);

        return Ok(warehouses.ToContract());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WarehouseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var warehouse = await _getWarehouseById.HandleAsync(new GetWarehouseByIdQuery { Id = id }, cancellationToken);

        return Ok(warehouse.ToContract());
    }

    [HttpPost]
    public async Task<ActionResult<WarehouseDto>> Create(CreateWarehouseRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateWarehouseCommand
        {
            Code = request.Code,
            Name = request.Name,
            Address = request.Address
        };

        var warehouse = await _createWarehouse.HandleAsync(command, cancellationToken);
        var contract = warehouse.ToContract();

        return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<WarehouseDto>> Update(Guid id, UpdateWarehouseRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateWarehouseCommand
        {
            Id = id,
            Code = request.Code,
            Name = request.Name,
            Address = request.Address
        };

        var warehouse = await _updateWarehouse.HandleAsync(command, cancellationToken);

        return Ok(warehouse.ToContract());
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<ActionResult<WarehouseDto>> Activate(Guid id, CancellationToken cancellationToken)
    {
        var warehouse = await _activateWarehouse.HandleAsync(new ActivateWarehouseCommand { Id = id }, cancellationToken);

        return Ok(warehouse.ToContract());
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<ActionResult<WarehouseDto>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var warehouse = await _deactivateWarehouse.HandleAsync(new DeactivateWarehouseCommand { Id = id }, cancellationToken);

        return Ok(warehouse.ToContract());
    }
}
