using Microsoft.AspNetCore.Mvc;
using WarehouseERP.Api.Contracts.Suppliers;
using WarehouseERP.Application.Common;
using WarehouseERP.Application.Procurement.Suppliers.Commands.ActivateSupplier;
using WarehouseERP.Application.Procurement.Suppliers.Commands.CreateSupplier;
using WarehouseERP.Application.Procurement.Suppliers.Commands.DeactivateSupplier;
using WarehouseERP.Application.Procurement.Suppliers.Commands.UpdateSupplier;
using WarehouseERP.Application.Procurement.Suppliers.Queries.GetSupplierById;
using WarehouseERP.Application.Procurement.Suppliers.Queries.GetSuppliers;
using WarehouseERP.Shared.Contracts.Suppliers;
using ApplicationSupplierDto = WarehouseERP.Application.Procurement.Suppliers.SupplierDto;

namespace WarehouseERP.Api.Controllers;

[ApiController]
[Route("api/suppliers")]
public sealed class SuppliersController : ControllerBase
{
    private readonly IQueryHandler<GetSuppliersQuery, IReadOnlyList<ApplicationSupplierDto>> _getSuppliers;
    private readonly IQueryHandler<GetSupplierByIdQuery, ApplicationSupplierDto> _getSupplierById;
    private readonly ICommandHandler<CreateSupplierCommand, ApplicationSupplierDto> _createSupplier;
    private readonly ICommandHandler<UpdateSupplierCommand, ApplicationSupplierDto> _updateSupplier;
    private readonly ICommandHandler<ActivateSupplierCommand, ApplicationSupplierDto> _activateSupplier;
    private readonly ICommandHandler<DeactivateSupplierCommand, ApplicationSupplierDto> _deactivateSupplier;

    public SuppliersController(
        IQueryHandler<GetSuppliersQuery, IReadOnlyList<ApplicationSupplierDto>> getSuppliers,
        IQueryHandler<GetSupplierByIdQuery, ApplicationSupplierDto> getSupplierById,
        ICommandHandler<CreateSupplierCommand, ApplicationSupplierDto> createSupplier,
        ICommandHandler<UpdateSupplierCommand, ApplicationSupplierDto> updateSupplier,
        ICommandHandler<ActivateSupplierCommand, ApplicationSupplierDto> activateSupplier,
        ICommandHandler<DeactivateSupplierCommand, ApplicationSupplierDto> deactivateSupplier)
    {
        _getSuppliers = getSuppliers;
        _getSupplierById = getSupplierById;
        _createSupplier = createSupplier;
        _updateSupplier = updateSupplier;
        _activateSupplier = activateSupplier;
        _deactivateSupplier = deactivateSupplier;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SupplierDto>>> GetAll(CancellationToken cancellationToken)
    {
        var suppliers = await _getSuppliers.HandleAsync(new GetSuppliersQuery(), cancellationToken);

        return Ok(suppliers.ToContract());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SupplierDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var supplier = await _getSupplierById.HandleAsync(new GetSupplierByIdQuery { Id = id }, cancellationToken);

        return Ok(supplier.ToContract());
    }

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> Create(CreateSupplierRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateSupplierCommand
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address
        };

        var supplier = await _createSupplier.HandleAsync(command, cancellationToken);
        var contract = supplier.ToContract();

        return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SupplierDto>> Update(Guid id, UpdateSupplierRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateSupplierCommand
        {
            Id = id,
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address
        };

        var supplier = await _updateSupplier.HandleAsync(command, cancellationToken);

        return Ok(supplier.ToContract());
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<ActionResult<SupplierDto>> Activate(Guid id, CancellationToken cancellationToken)
    {
        var supplier = await _activateSupplier.HandleAsync(new ActivateSupplierCommand { Id = id }, cancellationToken);

        return Ok(supplier.ToContract());
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<ActionResult<SupplierDto>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var supplier = await _deactivateSupplier.HandleAsync(new DeactivateSupplierCommand { Id = id }, cancellationToken);

        return Ok(supplier.ToContract());
    }
}
