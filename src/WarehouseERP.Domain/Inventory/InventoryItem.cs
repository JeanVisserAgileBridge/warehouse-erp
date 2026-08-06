using WarehouseERP.Domain.Exceptions;

namespace WarehouseERP.Domain.Inventory;

public class InventoryItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid StorageLocationId { get; private set; }
    public int QuantityOnHand { get; private set; }
    public int ReorderLevel { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InventoryItem()
    {
    }

    private InventoryItem(Guid id, Guid productId, Guid storageLocationId, int quantityOnHand, int reorderLevel)
    {
        Id = id;
        ProductId = productId;
        StorageLocationId = storageLocationId;
        QuantityOnHand = quantityOnHand;
        ReorderLevel = reorderLevel;
        UpdatedAt = DateTime.UtcNow;
    }

    public static InventoryItem Create(Guid productId, Guid storageLocationId, int quantityOnHand = 0, int reorderLevel = 0)
    {
        ValidateProductId(productId);
        ValidateStorageLocationId(storageLocationId);
        ValidateQuantityOnHand(quantityOnHand);
        ValidateReorderLevel(reorderLevel);

        return new InventoryItem(Guid.NewGuid(), productId, storageLocationId, quantityOnHand, reorderLevel);
    }

    public void ReceiveStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Received quantity must be greater than zero.");
        }

        QuantityOnHand += quantity;
        MarkUpdated();
    }

    public void IssueStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Issued quantity must be greater than zero.");
        }

        if (quantity > QuantityOnHand)
        {
            throw new DomainException("Issued quantity cannot reduce quantity on hand below zero.");
        }

        QuantityOnHand -= quantity;
        MarkUpdated();
    }

    public void AdjustStock(int newQuantityOnHand)
    {
        ValidateQuantityOnHand(newQuantityOnHand);
        QuantityOnHand = newQuantityOnHand;
        MarkUpdated();
    }

    public void ChangeReorderLevel(int reorderLevel)
    {
        ValidateReorderLevel(reorderLevel);
        ReorderLevel = reorderLevel;
        MarkUpdated();
    }

    private void MarkUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateProductId(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new DomainException("Inventory item must be assigned to a valid product.");
        }
    }

    private static void ValidateStorageLocationId(Guid storageLocationId)
    {
        if (storageLocationId == Guid.Empty)
        {
            throw new DomainException("Inventory item must be assigned to a valid storage location.");
        }
    }

    private static void ValidateQuantityOnHand(int quantityOnHand)
    {
        if (quantityOnHand < 0)
        {
            throw new DomainException("Quantity on hand cannot be negative.");
        }
    }

    private static void ValidateReorderLevel(int reorderLevel)
    {
        if (reorderLevel < 0)
        {
            throw new DomainException("Reorder level cannot be negative.");
        }
    }
}
