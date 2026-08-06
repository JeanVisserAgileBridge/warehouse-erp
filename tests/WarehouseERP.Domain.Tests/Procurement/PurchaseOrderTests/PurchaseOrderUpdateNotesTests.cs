using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.PurchaseOrderTests;

public class PurchaseOrderUpdateNotesTests
{
    [Fact]
    public void UpdateNotes_UpdatesNotes()
    {
        var purchaseOrder = CreateDraftOrder();

        purchaseOrder.UpdateNotes("Call before delivery");

        Assert.Equal("Call before delivery", purchaseOrder.Notes);
    }

    [Fact]
    public void UpdateNotes_AcceptsNull()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.UpdateNotes("Call before delivery");

        purchaseOrder.UpdateNotes(null);

        Assert.Null(purchaseOrder.Notes);
    }

    [Fact]
    public void UpdateNotes_UpdatesUpdatedAt()
    {
        var purchaseOrder = CreateDraftOrder();
        var originalUpdatedAt = purchaseOrder.UpdatedAt;

        purchaseOrder.UpdateNotes("Call before delivery");

        Assert.True(purchaseOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void UpdateNotes_RejectsNotesLongerThanMaxLength()
    {
        var purchaseOrder = CreateDraftOrder();
        var notes = new string('a', PurchaseOrder.MaxNotesLength + 1);

        Assert.Throws<DomainException>(() => purchaseOrder.UpdateNotes(notes));
    }

    [Fact]
    public void UpdateNotes_AcceptsNotesAtMaxLength()
    {
        var purchaseOrder = CreateDraftOrder();
        var notes = new string('a', PurchaseOrder.MaxNotesLength);

        purchaseOrder.UpdateNotes(notes);

        Assert.Equal(notes, purchaseOrder.Notes);
    }

    [Fact]
    public void UpdateNotes_ThrowsWhenOrderIsSubmitted()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        purchaseOrder.Submit();

        Assert.Throws<DomainException>(() => purchaseOrder.UpdateNotes("Too late"));
    }

    [Fact]
    public void UpdateNotes_ThrowsWhenOrderIsCancelled()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.Cancel();

        Assert.Throws<DomainException>(() => purchaseOrder.UpdateNotes("Too late"));
    }

    private static PurchaseOrder CreateDraftOrder()
    {
        return PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
    }
}
