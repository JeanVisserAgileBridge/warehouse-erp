using WarehouseERP.Domain.Exceptions;

namespace WarehouseERP.Domain.Sales;

public class SalesOrder
{
    public const int MaxOrderNumberLength = 50;
    public const int MaxNotesLength = 500;

    private readonly List<SalesOrderLine> _lines = new();

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public SalesOrderStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyCollection<SalesOrderLine> Lines => _lines.AsReadOnly();

    private SalesOrder()
    {
    }

    private SalesOrder(Guid id, Guid customerId, string orderNumber, DateTime orderDate, string? notes)
    {
        Id = id;
        CustomerId = customerId;
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        Notes = notes;
        Status = SalesOrderStatus.Draft;

        var now = DateTime.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public static SalesOrder Create(Guid customerId, string orderNumber, DateTime orderDate, string? notes = null)
    {
        ValidateCustomerId(customerId);
        ValidateOrderNumber(orderNumber);
        ValidateNotes(notes);

        return new SalesOrder(Guid.NewGuid(), customerId, orderNumber, orderDate, notes);
    }

    public void AddLine(Guid productId, int quantityOrdered, decimal unitPrice)
    {
        EnsureEditable();

        if (_lines.Any(line => line.ProductId == productId))
        {
            throw new DomainException("This product has already been added to the sales order.");
        }

        _lines.Add(SalesOrderLine.Create(Id, productId, quantityOrdered, unitPrice));
        MarkUpdated();
    }

    public void RemoveLine(Guid productId)
    {
        EnsureEditable();

        var line = FindLine(productId);
        _lines.Remove(line);
        MarkUpdated();
    }

    public void ChangeCustomer(Guid customerId)
    {
        EnsureEditable();
        ValidateCustomerId(customerId);

        CustomerId = customerId;
        MarkUpdated();
    }

    public void UpdateNotes(string? notes)
    {
        EnsureEditable();
        ValidateNotes(notes);

        Notes = notes;
        MarkUpdated();
    }

    public void Confirm()
    {
        if (Status != SalesOrderStatus.Draft)
        {
            throw new DomainException("Only draft sales orders can be confirmed.");
        }

        if (_lines.Count == 0)
        {
            throw new DomainException("A sales order must contain at least one line before it can be confirmed.");
        }

        Status = SalesOrderStatus.Confirmed;
        MarkUpdated();
    }

    public void Cancel()
    {
        if (Status == SalesOrderStatus.Fulfilled)
        {
            throw new DomainException("A fully fulfilled sales order cannot be cancelled.");
        }

        if (Status == SalesOrderStatus.Cancelled)
        {
            throw new DomainException("This sales order is already cancelled.");
        }

        Status = SalesOrderStatus.Cancelled;
        MarkUpdated();
    }

    public void FulfillProduct(Guid productId, int quantity)
    {
        if (Status != SalesOrderStatus.Confirmed && Status != SalesOrderStatus.PartiallyFulfilled)
        {
            throw new DomainException("Fulfilment can only occur against confirmed or partially fulfilled sales orders.");
        }

        var line = FindLine(productId);
        line.FulfillQuantity(quantity);

        Status = _lines.All(l => l.IsFullyFulfilled)
            ? SalesOrderStatus.Fulfilled
            : SalesOrderStatus.PartiallyFulfilled;

        MarkUpdated();
    }

    private SalesOrderLine FindLine(Guid productId)
    {
        var line = _lines.SingleOrDefault(l => l.ProductId == productId);

        if (line is null)
        {
            throw new DomainException("This product does not appear on the sales order.");
        }

        return line;
    }

    private void EnsureEditable()
    {
        if (Status != SalesOrderStatus.Draft)
        {
            throw new DomainException("Only draft sales orders can be edited.");
        }
    }

    private void MarkUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateCustomerId(Guid customerId)
    {
        if (customerId == Guid.Empty)
        {
            throw new DomainException("Sales order must be assigned to a valid customer.");
        }
    }

    private static void ValidateOrderNumber(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            throw new DomainException("Sales order number is required.");
        }

        if (orderNumber.Length > MaxOrderNumberLength)
        {
            throw new DomainException($"Sales order number cannot exceed {MaxOrderNumberLength} characters.");
        }
    }

    private static void ValidateNotes(string? notes)
    {
        if (notes is not null && notes.Length > MaxNotesLength)
        {
            throw new DomainException($"Sales order notes cannot exceed {MaxNotesLength} characters.");
        }
    }
}
