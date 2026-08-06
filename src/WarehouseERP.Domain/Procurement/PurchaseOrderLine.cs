using WarehouseERP.Domain.Exceptions;

namespace WarehouseERP.Domain.Procurement;

public class PurchaseOrderLine
{
    public Guid Id { get; private set; }
    public Guid PurchaseOrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int QuantityOrdered { get; private set; }
    public int QuantityReceived { get; private set; }
    public decimal UnitPrice { get; private set; }

    private PurchaseOrderLine()
    {
    }

    private PurchaseOrderLine(Guid id, Guid purchaseOrderId, Guid productId, int quantityOrdered, decimal unitPrice)
    {
        Id = id;
        PurchaseOrderId = purchaseOrderId;
        ProductId = productId;
        QuantityOrdered = quantityOrdered;
        UnitPrice = unitPrice;
        QuantityReceived = 0;
    }

    public static PurchaseOrderLine Create(Guid purchaseOrderId, Guid productId, int quantityOrdered, decimal unitPrice)
    {
        ValidateProductId(productId);
        ValidateQuantityOrdered(quantityOrdered);
        ValidateUnitPrice(unitPrice);

        return new PurchaseOrderLine(Guid.NewGuid(), purchaseOrderId, productId, quantityOrdered, unitPrice);
    }

    public void ReceiveQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Received quantity must be greater than zero.");
        }

        if (QuantityReceived + quantity > QuantityOrdered)
        {
            throw new DomainException("Received quantity cannot exceed quantity ordered.");
        }

        QuantityReceived += quantity;
    }

    internal bool IsFullyReceived => QuantityReceived == QuantityOrdered;

    private static void ValidateProductId(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new DomainException("Purchase order line must be assigned to a valid product.");
        }
    }

    private static void ValidateQuantityOrdered(int quantityOrdered)
    {
        if (quantityOrdered <= 0)
        {
            throw new DomainException("Quantity ordered must be greater than zero.");
        }
    }

    private static void ValidateUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 0)
        {
            throw new DomainException("Unit price cannot be negative.");
        }
    }
}
