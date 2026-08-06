using WarehouseERP.Domain.Exceptions;

namespace WarehouseERP.Domain.Sales;

public class SalesOrderLine
{
    public Guid Id { get; private set; }
    public Guid SalesOrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int QuantityOrdered { get; private set; }
    public int QuantityFulfilled { get; private set; }
    public decimal UnitPrice { get; private set; }

    private SalesOrderLine()
    {
    }

    private SalesOrderLine(Guid id, Guid salesOrderId, Guid productId, int quantityOrdered, decimal unitPrice)
    {
        Id = id;
        SalesOrderId = salesOrderId;
        ProductId = productId;
        QuantityOrdered = quantityOrdered;
        UnitPrice = unitPrice;
        QuantityFulfilled = 0;
    }

    public static SalesOrderLine Create(Guid salesOrderId, Guid productId, int quantityOrdered, decimal unitPrice)
    {
        ValidateProductId(productId);
        ValidateQuantityOrdered(quantityOrdered);
        ValidateUnitPrice(unitPrice);

        return new SalesOrderLine(Guid.NewGuid(), salesOrderId, productId, quantityOrdered, unitPrice);
    }

    public void FulfillQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Fulfilled quantity must be greater than zero.");
        }

        if (QuantityFulfilled + quantity > QuantityOrdered)
        {
            throw new DomainException("Fulfilled quantity cannot exceed quantity ordered.");
        }

        QuantityFulfilled += quantity;
    }

    internal bool IsFullyFulfilled => QuantityFulfilled == QuantityOrdered;

    private static void ValidateProductId(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new DomainException("Sales order line must be assigned to a valid product.");
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
