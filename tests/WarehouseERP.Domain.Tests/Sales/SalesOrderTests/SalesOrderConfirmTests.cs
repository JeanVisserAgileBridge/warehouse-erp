using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.SalesOrderTests;

public class SalesOrderConfirmTests
{
    [Fact]
    public void Confirm_ChangesStatusToConfirmed()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.AddLine(Guid.NewGuid(), 10, 4.50m);

        salesOrder.Confirm();

        Assert.Equal(SalesOrderStatus.Confirmed, salesOrder.Status);
    }

    [Fact]
    public void Confirm_UpdatesUpdatedAt()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        var originalUpdatedAt = salesOrder.UpdatedAt;

        salesOrder.Confirm();

        Assert.True(salesOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void Confirm_ThrowsWhenOrderHasNoLines()
    {
        var salesOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => salesOrder.Confirm());
    }

    [Fact]
    public void Confirm_DoesNotChangeStatusWhenOrderHasNoLines()
    {
        var salesOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => salesOrder.Confirm());

        Assert.Equal(SalesOrderStatus.Draft, salesOrder.Status);
    }

    [Fact]
    public void Confirm_ThrowsWhenOrderIsAlreadyConfirmed()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        salesOrder.Confirm();

        Assert.Throws<DomainException>(() => salesOrder.Confirm());
    }

    [Fact]
    public void Confirm_ThrowsWhenOrderIsCancelled()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.Cancel();

        Assert.Throws<DomainException>(() => salesOrder.Confirm());
    }

    private static SalesOrder CreateDraftOrder()
    {
        return SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
    }
}
