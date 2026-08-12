using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseERP.Api.Contracts.SalesOrders;
using WarehouseERP.Api.DependencyInjection;
using WarehouseERP.Application.Common;
using WarehouseERP.Application.Sales.SalesOrders.Commands.AddSalesOrderLine;
using WarehouseERP.Application.Sales.SalesOrders.Commands.CancelSalesOrder;
using WarehouseERP.Application.Sales.SalesOrders.Commands.ConfirmSalesOrder;
using WarehouseERP.Application.Sales.SalesOrders.Commands.CreateSalesOrder;
using WarehouseERP.Application.Sales.SalesOrders.Commands.FulfilSalesOrderLine;
using WarehouseERP.Application.Sales.SalesOrders.Commands.RemoveSalesOrderLine;
using WarehouseERP.Application.Sales.SalesOrders.Commands.UpdateSalesOrderLine;
using WarehouseERP.Application.Sales.SalesOrders.Queries.GetSalesOrderById;
using WarehouseERP.Application.Sales.SalesOrders.Queries.GetSalesOrders;
using WarehouseERP.Application.Sales.SalesOrders.Queries.GetSalesOrdersByCustomerId;
using WarehouseERP.Shared.Contracts.SalesOrders;
using ApplicationSalesOrderDto = WarehouseERP.Application.Sales.SalesOrders.SalesOrderDto;

namespace WarehouseERP.Api.Controllers;

[ApiController]
[Route("api/sales-orders")]
[Authorize(Policy = PolicyNames.SalesAccess)]
public sealed class SalesOrdersController : ControllerBase
{
    private readonly IQueryHandler<GetSalesOrdersQuery, IReadOnlyList<ApplicationSalesOrderDto>> _getSalesOrders;
    private readonly IQueryHandler<GetSalesOrderByIdQuery, ApplicationSalesOrderDto> _getSalesOrderById;
    private readonly IQueryHandler<GetSalesOrdersByCustomerIdQuery, IReadOnlyList<ApplicationSalesOrderDto>> _getSalesOrdersByCustomerId;
    private readonly ICommandHandler<CreateSalesOrderCommand, ApplicationSalesOrderDto> _createSalesOrder;
    private readonly ICommandHandler<AddSalesOrderLineCommand, ApplicationSalesOrderDto> _addSalesOrderLine;
    private readonly ICommandHandler<UpdateSalesOrderLineCommand, ApplicationSalesOrderDto> _updateSalesOrderLine;
    private readonly ICommandHandler<RemoveSalesOrderLineCommand, ApplicationSalesOrderDto> _removeSalesOrderLine;
    private readonly ICommandHandler<ConfirmSalesOrderCommand, ApplicationSalesOrderDto> _confirmSalesOrder;
    private readonly ICommandHandler<CancelSalesOrderCommand, ApplicationSalesOrderDto> _cancelSalesOrder;
    private readonly ICommandHandler<FulfilSalesOrderLineCommand, ApplicationSalesOrderDto> _fulfilSalesOrderLine;

    public SalesOrdersController(
        IQueryHandler<GetSalesOrdersQuery, IReadOnlyList<ApplicationSalesOrderDto>> getSalesOrders,
        IQueryHandler<GetSalesOrderByIdQuery, ApplicationSalesOrderDto> getSalesOrderById,
        IQueryHandler<GetSalesOrdersByCustomerIdQuery, IReadOnlyList<ApplicationSalesOrderDto>> getSalesOrdersByCustomerId,
        ICommandHandler<CreateSalesOrderCommand, ApplicationSalesOrderDto> createSalesOrder,
        ICommandHandler<AddSalesOrderLineCommand, ApplicationSalesOrderDto> addSalesOrderLine,
        ICommandHandler<UpdateSalesOrderLineCommand, ApplicationSalesOrderDto> updateSalesOrderLine,
        ICommandHandler<RemoveSalesOrderLineCommand, ApplicationSalesOrderDto> removeSalesOrderLine,
        ICommandHandler<ConfirmSalesOrderCommand, ApplicationSalesOrderDto> confirmSalesOrder,
        ICommandHandler<CancelSalesOrderCommand, ApplicationSalesOrderDto> cancelSalesOrder,
        ICommandHandler<FulfilSalesOrderLineCommand, ApplicationSalesOrderDto> fulfilSalesOrderLine)
    {
        _getSalesOrders = getSalesOrders;
        _getSalesOrderById = getSalesOrderById;
        _getSalesOrdersByCustomerId = getSalesOrdersByCustomerId;
        _createSalesOrder = createSalesOrder;
        _addSalesOrderLine = addSalesOrderLine;
        _updateSalesOrderLine = updateSalesOrderLine;
        _removeSalesOrderLine = removeSalesOrderLine;
        _confirmSalesOrder = confirmSalesOrder;
        _cancelSalesOrder = cancelSalesOrder;
        _fulfilSalesOrderLine = fulfilSalesOrderLine;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SalesOrderDto>>> GetAll(CancellationToken cancellationToken)
    {
        var salesOrders = await _getSalesOrders.HandleAsync(new GetSalesOrdersQuery(), cancellationToken);

        return Ok(salesOrders.ToContract());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SalesOrderDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var salesOrder = await _getSalesOrderById.HandleAsync(new GetSalesOrderByIdQuery { Id = id }, cancellationToken);

        return Ok(salesOrder.ToContract());
    }

    [HttpGet("/api/customers/{customerId:guid}/sales-orders")]
    public async Task<ActionResult<IReadOnlyList<SalesOrderDto>>> GetByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        var salesOrders = await _getSalesOrdersByCustomerId.HandleAsync(
            new GetSalesOrdersByCustomerIdQuery { CustomerId = customerId }, cancellationToken);

        return Ok(salesOrders.ToContract());
    }

    [HttpPost]
    public async Task<ActionResult<SalesOrderDto>> Create(CreateSalesOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateSalesOrderCommand
        {
            CustomerId = request.CustomerId,
            OrderNumber = request.OrderNumber,
            OrderDate = request.OrderDate,
            Notes = request.Notes
        };

        var salesOrder = await _createSalesOrder.HandleAsync(command, cancellationToken);
        var contract = salesOrder.ToContract();

        return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
    }

    [HttpPost("{id:guid}/lines")]
    public async Task<ActionResult<SalesOrderDto>> AddLine(Guid id, AddSalesOrderLineRequest request, CancellationToken cancellationToken)
    {
        var command = new AddSalesOrderLineCommand
        {
            SalesOrderId = id,
            ProductId = request.ProductId,
            QuantityOrdered = request.QuantityOrdered,
            UnitPrice = request.UnitPrice
        };

        var salesOrder = await _addSalesOrderLine.HandleAsync(command, cancellationToken);

        return Ok(salesOrder.ToContract());
    }

    [HttpPut("{id:guid}/lines/{productId:guid}")]
    public async Task<ActionResult<SalesOrderDto>> UpdateLine(
        Guid id, Guid productId, UpdateSalesOrderLineRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateSalesOrderLineCommand
        {
            SalesOrderId = id,
            ProductId = productId,
            QuantityOrdered = request.QuantityOrdered,
            UnitPrice = request.UnitPrice
        };

        var salesOrder = await _updateSalesOrderLine.HandleAsync(command, cancellationToken);

        return Ok(salesOrder.ToContract());
    }

    [HttpDelete("{id:guid}/lines/{productId:guid}")]
    public async Task<ActionResult<SalesOrderDto>> RemoveLine(Guid id, Guid productId, CancellationToken cancellationToken)
    {
        var salesOrder = await _removeSalesOrderLine.HandleAsync(
            new RemoveSalesOrderLineCommand { SalesOrderId = id, ProductId = productId }, cancellationToken);

        return Ok(salesOrder.ToContract());
    }

    [HttpPost("{id:guid}/confirm")]
    public async Task<ActionResult<SalesOrderDto>> Confirm(Guid id, CancellationToken cancellationToken)
    {
        var salesOrder = await _confirmSalesOrder.HandleAsync(
            new ConfirmSalesOrderCommand { SalesOrderId = id }, cancellationToken);

        return Ok(salesOrder.ToContract());
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<ActionResult<SalesOrderDto>> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var salesOrder = await _cancelSalesOrder.HandleAsync(
            new CancelSalesOrderCommand { SalesOrderId = id }, cancellationToken);

        return Ok(salesOrder.ToContract());
    }

    [HttpPost("{id:guid}/lines/{productId:guid}/fulfil")]
    public async Task<ActionResult<SalesOrderDto>> FulfilLine(
        Guid id, Guid productId, FulfilSalesOrderLineRequest request, CancellationToken cancellationToken)
    {
        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = id,
            ProductId = productId,
            Quantity = request.Quantity,
            StorageLocationId = request.StorageLocationId,
            Reference = request.Reference
        };

        var salesOrder = await _fulfilSalesOrderLine.HandleAsync(command, cancellationToken);

        return Ok(salesOrder.ToContract());
    }
}
