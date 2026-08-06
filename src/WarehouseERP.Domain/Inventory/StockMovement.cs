using WarehouseERP.Domain.Exceptions;

namespace WarehouseERP.Domain.Inventory;

public class StockMovement
{
    public const int MaxReferenceLength = 100;

    public Guid Id { get; private set; }
    public Guid InventoryItemId { get; private set; }
    public StockMovementType MovementType { get; private set; }
    public int Quantity { get; private set; }
    public string? Reference { get; private set; }
    public DateTime OccurredAt { get; private set; }

    private StockMovement()
    {
    }

    private StockMovement(Guid id, Guid inventoryItemId, StockMovementType movementType, int quantity, string? reference)
    {
        Id = id;
        InventoryItemId = inventoryItemId;
        MovementType = movementType;
        Quantity = quantity;
        Reference = reference;
        OccurredAt = DateTime.UtcNow;
    }

    public static StockMovement Create(Guid inventoryItemId, StockMovementType movementType, int quantity, string? reference = null)
    {
        ValidateInventoryItemId(inventoryItemId);
        ValidateQuantity(quantity);
        ValidateReference(reference);

        return new StockMovement(Guid.NewGuid(), inventoryItemId, movementType, quantity, reference);
    }

    private static void ValidateInventoryItemId(Guid inventoryItemId)
    {
        if (inventoryItemId == Guid.Empty)
        {
            throw new DomainException("Stock movement must be associated with a valid inventory item.");
        }
    }

    private static void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Stock movement quantity must be greater than zero.");
        }
    }

    private static void ValidateReference(string? reference)
    {
        if (reference is not null && reference.Length > MaxReferenceLength)
        {
            throw new DomainException($"Stock movement reference cannot exceed {MaxReferenceLength} characters.");
        }
    }
}
