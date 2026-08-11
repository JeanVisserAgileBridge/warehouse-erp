using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Inventory.StockMovements;

public sealed class StockMovementDto
{
    public required Guid Id { get; init; }
    public required Guid InventoryItemId { get; init; }
    public required StockMovementType MovementType { get; init; }
    public required int Quantity { get; init; }
    public string? Reference { get; init; }
    public required DateTime OccurredAt { get; init; }

    public static StockMovementDto FromDomain(StockMovement stockMovement)
    {
        return new StockMovementDto
        {
            Id = stockMovement.Id,
            InventoryItemId = stockMovement.InventoryItemId,
            MovementType = stockMovement.MovementType,
            Quantity = stockMovement.Quantity,
            Reference = stockMovement.Reference,
            OccurredAt = stockMovement.OccurredAt
        };
    }
}
