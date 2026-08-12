using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseERP.Api.Contracts.PurchaseOrders;
using WarehouseERP.Api.DependencyInjection;
using WarehouseERP.Application.Common;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.AddPurchaseOrderLine;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.CancelPurchaseOrder;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.CreatePurchaseOrder;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.ReceivePurchaseOrderLine;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.RemovePurchaseOrderLine;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.SubmitPurchaseOrder;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.UpdatePurchaseOrderLine;
using WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrderById;
using WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrders;
using WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrdersBySupplierId;
using WarehouseERP.Shared.Contracts.PurchaseOrders;
using ApplicationPurchaseOrderDto = WarehouseERP.Application.Procurement.PurchaseOrders.PurchaseOrderDto;

namespace WarehouseERP.Api.Controllers;

[ApiController]
[Route("api/purchase-orders")]
[Authorize(Policy = PolicyNames.PurchasingAccess)]
public sealed class PurchaseOrdersController : ControllerBase
{
    private readonly IQueryHandler<GetPurchaseOrdersQuery, IReadOnlyList<ApplicationPurchaseOrderDto>> _getPurchaseOrders;
    private readonly IQueryHandler<GetPurchaseOrderByIdQuery, ApplicationPurchaseOrderDto> _getPurchaseOrderById;
    private readonly IQueryHandler<GetPurchaseOrdersBySupplierIdQuery, IReadOnlyList<ApplicationPurchaseOrderDto>> _getPurchaseOrdersBySupplierId;
    private readonly ICommandHandler<CreatePurchaseOrderCommand, ApplicationPurchaseOrderDto> _createPurchaseOrder;
    private readonly ICommandHandler<AddPurchaseOrderLineCommand, ApplicationPurchaseOrderDto> _addPurchaseOrderLine;
    private readonly ICommandHandler<UpdatePurchaseOrderLineCommand, ApplicationPurchaseOrderDto> _updatePurchaseOrderLine;
    private readonly ICommandHandler<RemovePurchaseOrderLineCommand, ApplicationPurchaseOrderDto> _removePurchaseOrderLine;
    private readonly ICommandHandler<SubmitPurchaseOrderCommand, ApplicationPurchaseOrderDto> _submitPurchaseOrder;
    private readonly ICommandHandler<CancelPurchaseOrderCommand, ApplicationPurchaseOrderDto> _cancelPurchaseOrder;
    private readonly ICommandHandler<ReceivePurchaseOrderLineCommand, ApplicationPurchaseOrderDto> _receivePurchaseOrderLine;

    public PurchaseOrdersController(
        IQueryHandler<GetPurchaseOrdersQuery, IReadOnlyList<ApplicationPurchaseOrderDto>> getPurchaseOrders,
        IQueryHandler<GetPurchaseOrderByIdQuery, ApplicationPurchaseOrderDto> getPurchaseOrderById,
        IQueryHandler<GetPurchaseOrdersBySupplierIdQuery, IReadOnlyList<ApplicationPurchaseOrderDto>> getPurchaseOrdersBySupplierId,
        ICommandHandler<CreatePurchaseOrderCommand, ApplicationPurchaseOrderDto> createPurchaseOrder,
        ICommandHandler<AddPurchaseOrderLineCommand, ApplicationPurchaseOrderDto> addPurchaseOrderLine,
        ICommandHandler<UpdatePurchaseOrderLineCommand, ApplicationPurchaseOrderDto> updatePurchaseOrderLine,
        ICommandHandler<RemovePurchaseOrderLineCommand, ApplicationPurchaseOrderDto> removePurchaseOrderLine,
        ICommandHandler<SubmitPurchaseOrderCommand, ApplicationPurchaseOrderDto> submitPurchaseOrder,
        ICommandHandler<CancelPurchaseOrderCommand, ApplicationPurchaseOrderDto> cancelPurchaseOrder,
        ICommandHandler<ReceivePurchaseOrderLineCommand, ApplicationPurchaseOrderDto> receivePurchaseOrderLine)
    {
        _getPurchaseOrders = getPurchaseOrders;
        _getPurchaseOrderById = getPurchaseOrderById;
        _getPurchaseOrdersBySupplierId = getPurchaseOrdersBySupplierId;
        _createPurchaseOrder = createPurchaseOrder;
        _addPurchaseOrderLine = addPurchaseOrderLine;
        _updatePurchaseOrderLine = updatePurchaseOrderLine;
        _removePurchaseOrderLine = removePurchaseOrderLine;
        _submitPurchaseOrder = submitPurchaseOrder;
        _cancelPurchaseOrder = cancelPurchaseOrder;
        _receivePurchaseOrderLine = receivePurchaseOrderLine;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PurchaseOrderDto>>> GetAll(CancellationToken cancellationToken)
    {
        var purchaseOrders = await _getPurchaseOrders.HandleAsync(new GetPurchaseOrdersQuery(), cancellationToken);

        return Ok(purchaseOrders.ToContract());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PurchaseOrderDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _getPurchaseOrderById.HandleAsync(new GetPurchaseOrderByIdQuery { Id = id }, cancellationToken);

        return Ok(purchaseOrder.ToContract());
    }

    [HttpGet("/api/suppliers/{supplierId:guid}/purchase-orders")]
    public async Task<ActionResult<IReadOnlyList<PurchaseOrderDto>>> GetBySupplierId(Guid supplierId, CancellationToken cancellationToken)
    {
        var purchaseOrders = await _getPurchaseOrdersBySupplierId.HandleAsync(
            new GetPurchaseOrdersBySupplierIdQuery { SupplierId = supplierId }, cancellationToken);

        return Ok(purchaseOrders.ToContract());
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseOrderDto>> Create(CreatePurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CreatePurchaseOrderCommand
        {
            SupplierId = request.SupplierId,
            OrderNumber = request.OrderNumber,
            OrderDate = request.OrderDate,
            Notes = request.Notes
        };

        var purchaseOrder = await _createPurchaseOrder.HandleAsync(command, cancellationToken);
        var contract = purchaseOrder.ToContract();

        return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
    }

    [HttpPost("{id:guid}/lines")]
    public async Task<ActionResult<PurchaseOrderDto>> AddLine(Guid id, AddPurchaseOrderLineRequest request, CancellationToken cancellationToken)
    {
        var command = new AddPurchaseOrderLineCommand
        {
            PurchaseOrderId = id,
            ProductId = request.ProductId,
            QuantityOrdered = request.QuantityOrdered,
            UnitPrice = request.UnitPrice
        };

        var purchaseOrder = await _addPurchaseOrderLine.HandleAsync(command, cancellationToken);

        return Ok(purchaseOrder.ToContract());
    }

    [HttpPut("{id:guid}/lines/{productId:guid}")]
    public async Task<ActionResult<PurchaseOrderDto>> UpdateLine(
        Guid id, Guid productId, UpdatePurchaseOrderLineRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdatePurchaseOrderLineCommand
        {
            PurchaseOrderId = id,
            ProductId = productId,
            QuantityOrdered = request.QuantityOrdered,
            UnitPrice = request.UnitPrice
        };

        var purchaseOrder = await _updatePurchaseOrderLine.HandleAsync(command, cancellationToken);

        return Ok(purchaseOrder.ToContract());
    }

    [HttpDelete("{id:guid}/lines/{productId:guid}")]
    public async Task<ActionResult<PurchaseOrderDto>> RemoveLine(Guid id, Guid productId, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _removePurchaseOrderLine.HandleAsync(
            new RemovePurchaseOrderLineCommand { PurchaseOrderId = id, ProductId = productId }, cancellationToken);

        return Ok(purchaseOrder.ToContract());
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<ActionResult<PurchaseOrderDto>> Submit(Guid id, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _submitPurchaseOrder.HandleAsync(
            new SubmitPurchaseOrderCommand { PurchaseOrderId = id }, cancellationToken);

        return Ok(purchaseOrder.ToContract());
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<ActionResult<PurchaseOrderDto>> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _cancelPurchaseOrder.HandleAsync(
            new CancelPurchaseOrderCommand { PurchaseOrderId = id }, cancellationToken);

        return Ok(purchaseOrder.ToContract());
    }

    [HttpPost("{id:guid}/lines/{productId:guid}/receive")]
    public async Task<ActionResult<PurchaseOrderDto>> ReceiveLine(
        Guid id, Guid productId, ReceivePurchaseOrderLineRequest request, CancellationToken cancellationToken)
    {
        var command = new ReceivePurchaseOrderLineCommand
        {
            PurchaseOrderId = id,
            ProductId = productId,
            Quantity = request.Quantity,
            StorageLocationId = request.StorageLocationId,
            Reference = request.Reference
        };

        var purchaseOrder = await _receivePurchaseOrderLine.HandleAsync(command, cancellationToken);

        return Ok(purchaseOrder.ToContract());
    }
}
