using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.PurchaseOrderTests;

public class PurchaseOrderCreateTests
{
    [Fact]
    public void Create_ReturnsPurchaseOrderWithNonEmptyGuid()
    {
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);

        Assert.NotEqual(Guid.Empty, purchaseOrder.Id);
    }

    [Fact]
    public void Create_StoresSuppliedValues()
    {
        var supplierId = Guid.NewGuid();
        var orderDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc);

        var purchaseOrder = PurchaseOrder.Create(supplierId, "PO-001", orderDate, "Rush order");

        Assert.Equal(supplierId, purchaseOrder.SupplierId);
        Assert.Equal("PO-001", purchaseOrder.OrderNumber);
        Assert.Equal(orderDate, purchaseOrder.OrderDate);
        Assert.Equal("Rush order", purchaseOrder.Notes);
    }

    [Fact]
    public void Create_StartsInDraftStatus()
    {
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);

        Assert.Equal(PurchaseOrderStatus.Draft, purchaseOrder.Status);
    }

    [Fact]
    public void Create_StartsWithNoLines()
    {
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);

        Assert.Empty(purchaseOrder.Lines);
    }

    [Fact]
    public void Create_SetsCreatedAtAndUpdatedAtToSameValue()
    {
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);

        Assert.Equal(purchaseOrder.CreatedAt, purchaseOrder.UpdatedAt);
    }

    [Fact]
    public void Create_AcceptsNullNotes()
    {
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);

        Assert.Null(purchaseOrder.Notes);
    }

    [Fact]
    public void Create_RejectsEmptySupplierId()
    {
        Assert.Throws<DomainException>(() => PurchaseOrder.Create(Guid.Empty, "PO-001", DateTime.UtcNow));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_RejectsNullEmptyOrWhitespaceOrderNumber(string? orderNumber)
    {
        Assert.Throws<DomainException>(() => PurchaseOrder.Create(Guid.NewGuid(), orderNumber!, DateTime.UtcNow));
    }

    [Fact]
    public void Create_RejectsOrderNumberLongerThanMaxLength()
    {
        var orderNumber = new string('a', PurchaseOrder.MaxOrderNumberLength + 1);

        Assert.Throws<DomainException>(() => PurchaseOrder.Create(Guid.NewGuid(), orderNumber, DateTime.UtcNow));
    }

    [Fact]
    public void Create_AcceptsOrderNumberAtMaxLength()
    {
        var orderNumber = new string('a', PurchaseOrder.MaxOrderNumberLength);

        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), orderNumber, DateTime.UtcNow);

        Assert.Equal(orderNumber, purchaseOrder.OrderNumber);
    }

    [Fact]
    public void Create_RejectsNotesLongerThanMaxLength()
    {
        var notes = new string('a', PurchaseOrder.MaxNotesLength + 1);

        Assert.Throws<DomainException>(() => PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow, notes));
    }

    [Fact]
    public void Create_AcceptsNotesAtMaxLength()
    {
        var notes = new string('a', PurchaseOrder.MaxNotesLength);

        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow, notes);

        Assert.Equal(notes, purchaseOrder.Notes);
    }
}
