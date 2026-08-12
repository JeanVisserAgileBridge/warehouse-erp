using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseERP.Api.Contracts.StorageLocations;
using WarehouseERP.Api.DependencyInjection;
using WarehouseERP.Application.Common;
using WarehouseERP.Application.Warehouses.StorageLocations.Commands.ActivateStorageLocation;
using WarehouseERP.Application.Warehouses.StorageLocations.Commands.CreateStorageLocation;
using WarehouseERP.Application.Warehouses.StorageLocations.Commands.DeactivateStorageLocation;
using WarehouseERP.Application.Warehouses.StorageLocations.Commands.UpdateStorageLocation;
using WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocationById;
using WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocations;
using WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocationsByWarehouseId;
using WarehouseERP.Shared.Contracts.StorageLocations;
using ApplicationStorageLocationDto = WarehouseERP.Application.Warehouses.StorageLocations.StorageLocationDto;

namespace WarehouseERP.Api.Controllers;

[ApiController]
[Route("api/storage-locations")]
public sealed class StorageLocationsController : ControllerBase
{
    private readonly IQueryHandler<GetStorageLocationsQuery, IReadOnlyList<ApplicationStorageLocationDto>> _getStorageLocations;
    private readonly IQueryHandler<GetStorageLocationByIdQuery, ApplicationStorageLocationDto> _getStorageLocationById;
    private readonly IQueryHandler<GetStorageLocationsByWarehouseIdQuery, IReadOnlyList<ApplicationStorageLocationDto>> _getStorageLocationsByWarehouseId;
    private readonly ICommandHandler<CreateStorageLocationCommand, ApplicationStorageLocationDto> _createStorageLocation;
    private readonly ICommandHandler<UpdateStorageLocationCommand, ApplicationStorageLocationDto> _updateStorageLocation;
    private readonly ICommandHandler<ActivateStorageLocationCommand, ApplicationStorageLocationDto> _activateStorageLocation;
    private readonly ICommandHandler<DeactivateStorageLocationCommand, ApplicationStorageLocationDto> _deactivateStorageLocation;

    public StorageLocationsController(
        IQueryHandler<GetStorageLocationsQuery, IReadOnlyList<ApplicationStorageLocationDto>> getStorageLocations,
        IQueryHandler<GetStorageLocationByIdQuery, ApplicationStorageLocationDto> getStorageLocationById,
        IQueryHandler<GetStorageLocationsByWarehouseIdQuery, IReadOnlyList<ApplicationStorageLocationDto>> getStorageLocationsByWarehouseId,
        ICommandHandler<CreateStorageLocationCommand, ApplicationStorageLocationDto> createStorageLocation,
        ICommandHandler<UpdateStorageLocationCommand, ApplicationStorageLocationDto> updateStorageLocation,
        ICommandHandler<ActivateStorageLocationCommand, ApplicationStorageLocationDto> activateStorageLocation,
        ICommandHandler<DeactivateStorageLocationCommand, ApplicationStorageLocationDto> deactivateStorageLocation)
    {
        _getStorageLocations = getStorageLocations;
        _getStorageLocationById = getStorageLocationById;
        _getStorageLocationsByWarehouseId = getStorageLocationsByWarehouseId;
        _createStorageLocation = createStorageLocation;
        _updateStorageLocation = updateStorageLocation;
        _activateStorageLocation = activateStorageLocation;
        _deactivateStorageLocation = deactivateStorageLocation;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StorageLocationDto>>> GetAll(CancellationToken cancellationToken)
    {
        var storageLocations = await _getStorageLocations.HandleAsync(new GetStorageLocationsQuery(), cancellationToken);

        return Ok(storageLocations.ToContract());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StorageLocationDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var storageLocation = await _getStorageLocationById.HandleAsync(new GetStorageLocationByIdQuery { Id = id }, cancellationToken);

        return Ok(storageLocation.ToContract());
    }

    [HttpGet("/api/warehouses/{warehouseId:guid}/storage-locations")]
    public async Task<ActionResult<IReadOnlyList<StorageLocationDto>>> GetByWarehouseId(Guid warehouseId, CancellationToken cancellationToken)
    {
        var storageLocations = await _getStorageLocationsByWarehouseId.HandleAsync(
            new GetStorageLocationsByWarehouseIdQuery { WarehouseId = warehouseId }, cancellationToken);

        return Ok(storageLocations.ToContract());
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.WarehouseAccess)]
    public async Task<ActionResult<StorageLocationDto>> Create(CreateStorageLocationRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateStorageLocationCommand
        {
            WarehouseId = request.WarehouseId,
            Code = request.Code,
            Description = request.Description
        };

        var storageLocation = await _createStorageLocation.HandleAsync(command, cancellationToken);
        var contract = storageLocation.ToContract();

        return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyNames.WarehouseAccess)]
    public async Task<ActionResult<StorageLocationDto>> Update(Guid id, UpdateStorageLocationRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateStorageLocationCommand
        {
            Id = id,
            Code = request.Code,
            Description = request.Description
        };

        var storageLocation = await _updateStorageLocation.HandleAsync(command, cancellationToken);

        return Ok(storageLocation.ToContract());
    }

    [HttpPatch("{id:guid}/activate")]
    [Authorize(Policy = PolicyNames.WarehouseAccess)]
    public async Task<ActionResult<StorageLocationDto>> Activate(Guid id, CancellationToken cancellationToken)
    {
        var storageLocation = await _activateStorageLocation.HandleAsync(new ActivateStorageLocationCommand { Id = id }, cancellationToken);

        return Ok(storageLocation.ToContract());
    }

    [HttpPatch("{id:guid}/deactivate")]
    [Authorize(Policy = PolicyNames.WarehouseAccess)]
    public async Task<ActionResult<StorageLocationDto>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var storageLocation = await _deactivateStorageLocation.HandleAsync(new DeactivateStorageLocationCommand { Id = id }, cancellationToken);

        return Ok(storageLocation.ToContract());
    }
}
