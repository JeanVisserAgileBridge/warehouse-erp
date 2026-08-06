using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.SalesOrderTests;

public class SalesOrderUpdateNotesTests
{
    [Fact]
    public void UpdateNotes_UpdatesNotes()
    {
        var salesOrder = CreateDraftOrder();

        salesOrder.UpdateNotes("Call before delivery");

        Assert.Equal("Call before delivery", salesOrder.Notes);
    }

    [Fact]
    public void UpdateNotes_AcceptsNull()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.UpdateNotes("Call before delivery");

        salesOrder.UpdateNotes(null);

        Assert.Null(salesOrder.Notes);
    }

    [Fact]
    public void UpdateNotes_UpdatesUpdatedAt()
    {
        var salesOrder = CreateDraftOrder();
        var originalUpdatedAt = salesOrder.UpdatedAt;

        salesOrder.UpdateNotes("Call before delivery");

        Assert.True(salesOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void UpdateNotes_RejectsNotesLongerThanMaxLength()
    {
        var salesOrder = CreateDraftOrder();
        var notes = new string('a', SalesOrder.MaxNotesLength + 1);

        Assert.Throws<DomainException>(() => salesOrder.UpdateNotes(notes));
    }

    [Fact]
    public void UpdateNotes_AcceptsNotesAtMaxLength()
    {
        var salesOrder = CreateDraftOrder();
        var notes = new string('a', SalesOrder.MaxNotesLength);

        salesOrder.UpdateNotes(notes);

        Assert.Equal(notes, salesOrder.Notes);
    }

    [Fact]
    public void UpdateNotes_ThrowsWhenOrderIsConfirmed()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        salesOrder.Confirm();

        Assert.Throws<DomainException>(() => salesOrder.UpdateNotes("Too late"));
    }

    [Fact]
    public void UpdateNotes_ThrowsWhenOrderIsCancelled()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.Cancel();

        Assert.Throws<DomainException>(() => salesOrder.UpdateNotes("Too late"));
    }

    private static SalesOrder CreateDraftOrder()
    {
        return SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
    }
}
