using Microsoft.AspNetCore.Mvc;
using WarehouseERP.Api.Contracts.Inventory;
using WarehouseERP.Application.Common;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.AdjustStock;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.ChangeReorderLevel;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.CreateInventoryItem;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.IssueStock;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.ReceiveStock;
using WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryByProductId;
using WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryByStorageLocationId;
using WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryItemById;
using WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryItems;
using WarehouseERP.Application.Inventory.StockMovements.Queries.GetStockMovementsByInventoryItemId;
using WarehouseERP.Shared.Contracts.Inventory;
using ApplicationInventoryItemDto = WarehouseERP.Application.Inventory.InventoryItems.InventoryItemDto;
using ApplicationStockMovementDto = WarehouseERP.Application.Inventory.StockMovements.StockMovementDto;

namespace WarehouseERP.Api.Controllers;

[ApiController]
[Route("api/inventory")]
public sealed class InventoryController : ControllerBase
{
    private readonly IQueryHandler<GetInventoryItemsQuery, IReadOnlyList<ApplicationInventoryItemDto>> _getInventoryItems;
    private readonly IQueryHandler<GetInventoryItemByIdQuery, ApplicationInventoryItemDto> _getInventoryItemById;
    private readonly IQueryHandler<GetInventoryByProductIdQuery, IReadOnlyList<ApplicationInventoryItemDto>> _getInventoryByProductId;
    private readonly IQueryHandler<GetInventoryByStorageLocationIdQuery, IReadOnlyList<ApplicationInventoryItemDto>> _getInventoryByStorageLocationId;
    private readonly IQueryHandler<GetStockMovementsByInventoryItemIdQuery, IReadOnlyList<ApplicationStockMovementDto>> _getStockMovementsByInventoryItemId;
    private readonly ICommandHandler<CreateInventoryItemCommand, ApplicationInventoryItemDto> _createInventoryItem;
    private readonly ICommandHandler<ReceiveStockCommand, ApplicationInventoryItemDto> _receiveStock;
    private readonly ICommandHandler<IssueStockCommand, ApplicationInventoryItemDto> _issueStock;
    private readonly ICommandHandler<AdjustStockCommand, ApplicationInventoryItemDto> _adjustStock;
    private readonly ICommandHandler<ChangeReorderLevelCommand, ApplicationInventoryItemDto> _changeReorderLevel;

    public InventoryController(
        IQueryHandler<GetInventoryItemsQuery, IReadOnlyList<ApplicationInventoryItemDto>> getInventoryItems,
        IQueryHandler<GetInventoryItemByIdQuery, ApplicationInventoryItemDto> getInventoryItemById,
        IQueryHandler<GetInventoryByProductIdQuery, IReadOnlyList<ApplicationInventoryItemDto>> getInventoryByProductId,
        IQueryHandler<GetInventoryByStorageLocationIdQuery, IReadOnlyList<ApplicationInventoryItemDto>> getInventoryByStorageLocationId,
        IQueryHandler<GetStockMovementsByInventoryItemIdQuery, IReadOnlyList<ApplicationStockMovementDto>> getStockMovementsByInventoryItemId,
        ICommandHandler<CreateInventoryItemCommand, ApplicationInventoryItemDto> createInventoryItem,
        ICommandHandler<ReceiveStockCommand, ApplicationInventoryItemDto> receiveStock,
        ICommandHandler<IssueStockCommand, ApplicationInventoryItemDto> issueStock,
        ICommandHandler<AdjustStockCommand, ApplicationInventoryItemDto> adjustStock,
        ICommandHandler<ChangeReorderLevelCommand, ApplicationInventoryItemDto> changeReorderLevel)
    {
        _getInventoryItems = getInventoryItems;
        _getInventoryItemById = getInventoryItemById;
        _getInventoryByProductId = getInventoryByProductId;
        _getInventoryByStorageLocationId = getInventoryByStorageLocationId;
        _getStockMovementsByInventoryItemId = getStockMovementsByInventoryItemId;
        _createInventoryItem = createInventoryItem;
        _receiveStock = receiveStock;
        _issueStock = issueStock;
        _adjustStock = adjustStock;
        _changeReorderLevel = changeReorderLevel;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<InventoryItemDto>>> GetAll(CancellationToken cancellationToken)
    {
        var inventoryItems = await _getInventoryItems.HandleAsync(new GetInventoryItemsQuery(), cancellationToken);

        return Ok(inventoryItems.ToContract());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InventoryItemDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var inventoryItem = await _getInventoryItemById.HandleAsync(new GetInventoryItemByIdQuery { Id = id }, cancellationToken);

        return Ok(inventoryItem.ToContract());
    }

    [HttpGet("product/{productId:guid}")]
    public async Task<ActionResult<IReadOnlyList<InventoryItemDto>>> GetByProductId(Guid productId, CancellationToken cancellationToken)
    {
        var inventoryItems = await _getInventoryByProductId.HandleAsync(
            new GetInventoryByProductIdQuery { ProductId = productId }, cancellationToken);

        return Ok(inventoryItems.ToContract());
    }

    [HttpGet("storage-location/{storageLocationId:guid}")]
    public async Task<ActionResult<IReadOnlyList<InventoryItemDto>>> GetByStorageLocationId(Guid storageLocationId, CancellationToken cancellationToken)
    {
        var inventoryItems = await _getInventoryByStorageLocationId.HandleAsync(
            new GetInventoryByStorageLocationIdQuery { StorageLocationId = storageLocationId }, cancellationToken);

        return Ok(inventoryItems.ToContract());
    }

    [HttpGet("{id:guid}/movements")]
    public async Task<ActionResult<IReadOnlyList<StockMovementDto>>> GetMovements(Guid id, CancellationToken cancellationToken)
    {
        var stockMovements = await _getStockMovementsByInventoryItemId.HandleAsync(
            new GetStockMovementsByInventoryItemIdQuery { InventoryItemId = id }, cancellationToken);

        return Ok(stockMovements.ToContract());
    }

    [HttpPost]
    public async Task<ActionResult<InventoryItemDto>> Create(CreateInventoryItemRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateInventoryItemCommand
        {
            ProductId = request.ProductId,
            StorageLocationId = request.StorageLocationId,
            QuantityOnHand = request.QuantityOnHand,
            ReorderLevel = request.ReorderLevel
        };

        var inventoryItem = await _createInventoryItem.HandleAsync(command, cancellationToken);
        var contract = inventoryItem.ToContract();

        return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
    }

    [HttpPost("{id:guid}/receive")]
    public async Task<ActionResult<InventoryItemDto>> Receive(Guid id, ReceiveStockRequest request, CancellationToken cancellationToken)
    {
        var command = new ReceiveStockCommand
        {
            InventoryItemId = id,
            Quantity = request.Quantity,
            Reference = request.Reference
        };

        var inventoryItem = await _receiveStock.HandleAsync(command, cancellationToken);

        return Ok(inventoryItem.ToContract());
    }

    [HttpPost("{id:guid}/issue")]
    public async Task<ActionResult<InventoryItemDto>> Issue(Guid id, IssueStockRequest request, CancellationToken cancellationToken)
    {
        var command = new IssueStockCommand
        {
            InventoryItemId = id,
            Quantity = request.Quantity,
            Reference = request.Reference
        };

        var inventoryItem = await _issueStock.HandleAsync(command, cancellationToken);

        return Ok(inventoryItem.ToContract());
    }

    [HttpPost("{id:guid}/adjust")]
    public async Task<ActionResult<InventoryItemDto>> Adjust(Guid id, AdjustStockRequest request, CancellationToken cancellationToken)
    {
        var command = new AdjustStockCommand
        {
            InventoryItemId = id,
            NewQuantityOnHand = request.NewQuantityOnHand,
            Reference = request.Reference
        };

        var inventoryItem = await _adjustStock.HandleAsync(command, cancellationToken);

        return Ok(inventoryItem.ToContract());
    }

    [HttpPatch("{id:guid}/reorder-level")]
    public async Task<ActionResult<InventoryItemDto>> ChangeReorderLevel(Guid id, ChangeReorderLevelRequest request, CancellationToken cancellationToken)
    {
        var command = new ChangeReorderLevelCommand
        {
            InventoryItemId = id,
            ReorderLevel = request.ReorderLevel
        };

        var inventoryItem = await _changeReorderLevel.HandleAsync(command, cancellationToken);

        return Ok(inventoryItem.ToContract());
    }
}
