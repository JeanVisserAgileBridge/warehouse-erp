using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Inventory.InventoryItems;

public sealed class InventoryItemDto
{
    public required Guid Id { get; init; }
    public required Guid ProductId { get; init; }
    public required Guid StorageLocationId { get; init; }
    public required int QuantityOnHand { get; init; }
    public required int ReorderLevel { get; init; }
    public required DateTime UpdatedAt { get; init; }

    public static InventoryItemDto FromDomain(InventoryItem inventoryItem)
    {
        return new InventoryItemDto
        {
            Id = inventoryItem.Id,
            ProductId = inventoryItem.ProductId,
            StorageLocationId = inventoryItem.StorageLocationId,
            QuantityOnHand = inventoryItem.QuantityOnHand,
            ReorderLevel = inventoryItem.ReorderLevel,
            UpdatedAt = inventoryItem.UpdatedAt
        };
    }
}
