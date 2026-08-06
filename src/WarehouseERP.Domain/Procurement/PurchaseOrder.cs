using WarehouseERP.Domain.Exceptions;

namespace WarehouseERP.Domain.Procurement;

public class PurchaseOrder
{
    public const int MaxOrderNumberLength = 50;
    public const int MaxNotesLength = 500;

    private readonly List<PurchaseOrderLine> _lines = new();

    public Guid Id { get; private set; }
    public Guid SupplierId { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public PurchaseOrderStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyCollection<PurchaseOrderLine> Lines => _lines.AsReadOnly();

    private PurchaseOrder()
    {
    }

    private PurchaseOrder(Guid id, Guid supplierId, string orderNumber, DateTime orderDate, string? notes)
    {
        Id = id;
        SupplierId = supplierId;
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        Notes = notes;
        Status = PurchaseOrderStatus.Draft;

        var now = DateTime.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public static PurchaseOrder Create(Guid supplierId, string orderNumber, DateTime orderDate, string? notes = null)
    {
        ValidateSupplierId(supplierId);
        ValidateOrderNumber(orderNumber);
        ValidateNotes(notes);

        return new PurchaseOrder(Guid.NewGuid(), supplierId, orderNumber, orderDate, notes);
    }

    public void AddLine(Guid productId, int quantityOrdered, decimal unitPrice)
    {
        EnsureEditable();

        if (_lines.Any(line => line.ProductId == productId))
        {
            throw new DomainException("This product has already been added to the purchase order.");
        }

        _lines.Add(PurchaseOrderLine.Create(Id, productId, quantityOrdered, unitPrice));
        MarkUpdated();
    }

    public void RemoveLine(Guid productId)
    {
        EnsureEditable();

        var line = FindLine(productId);
        _lines.Remove(line);
        MarkUpdated();
    }

    public void ChangeSupplier(Guid supplierId)
    {
        EnsureEditable();
        ValidateSupplierId(supplierId);

        SupplierId = supplierId;
        MarkUpdated();
    }

    public void UpdateNotes(string? notes)
    {
        EnsureEditable();
        ValidateNotes(notes);

        Notes = notes;
        MarkUpdated();
    }

    public void Submit()
    {
        if (Status != PurchaseOrderStatus.Draft)
        {
            throw new DomainException("Only draft purchase orders can be submitted.");
        }

        if (_lines.Count == 0)
        {
            throw new DomainException("A purchase order must contain at least one line before it can be submitted.");
        }

        Status = PurchaseOrderStatus.Submitted;
        MarkUpdated();
    }

    public void Cancel()
    {
        if (Status == PurchaseOrderStatus.Received)
        {
            throw new DomainException("A fully received purchase order cannot be cancelled.");
        }

        if (Status == PurchaseOrderStatus.Cancelled)
        {
            throw new DomainException("This purchase order is already cancelled.");
        }

        Status = PurchaseOrderStatus.Cancelled;
        MarkUpdated();
    }

    public void ReceiveProduct(Guid productId, int quantity)
    {
        if (Status != PurchaseOrderStatus.Submitted && Status != PurchaseOrderStatus.PartiallyReceived)
        {
            throw new DomainException("Stock can only be received against submitted or partially received purchase orders.");
        }

        var line = FindLine(productId);
        line.ReceiveQuantity(quantity);

        Status = _lines.All(l => l.IsFullyReceived)
            ? PurchaseOrderStatus.Received
            : PurchaseOrderStatus.PartiallyReceived;

        MarkUpdated();
    }

    private PurchaseOrderLine FindLine(Guid productId)
    {
        var line = _lines.SingleOrDefault(l => l.ProductId == productId);

        if (line is null)
        {
            throw new DomainException("This product does not appear on the purchase order.");
        }

        return line;
    }

    private void EnsureEditable()
    {
        if (Status != PurchaseOrderStatus.Draft)
        {
            throw new DomainException("Only draft purchase orders can be edited.");
        }
    }

    private void MarkUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateSupplierId(Guid supplierId)
    {
        if (supplierId == Guid.Empty)
        {
            throw new DomainException("Purchase order must be assigned to a valid supplier.");
        }
    }

    private static void ValidateOrderNumber(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            throw new DomainException("Purchase order number is required.");
        }

        if (orderNumber.Length > MaxOrderNumberLength)
        {
            throw new DomainException($"Purchase order number cannot exceed {MaxOrderNumberLength} characters.");
        }
    }

    private static void ValidateNotes(string? notes)
    {
        if (notes is not null && notes.Length > MaxNotesLength)
        {
            throw new DomainException($"Purchase order notes cannot exceed {MaxNotesLength} characters.");
        }
    }
}
