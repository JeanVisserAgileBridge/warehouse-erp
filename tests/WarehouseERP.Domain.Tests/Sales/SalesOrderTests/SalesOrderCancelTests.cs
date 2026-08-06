using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.SalesOrderTests;

public class SalesOrderCancelTests
{
    [Fact]
    public void Cancel_ChangesStatusToCancelledFromDraft()
    {
        var salesOrder = CreateDraftOrder();

        salesOrder.Cancel();

        Assert.Equal(SalesOrderStatus.Cancelled, salesOrder.Status);
    }

    [Fact]
    public void Cancel_ChangesStatusToCancelledFromConfirmed()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        salesOrder.Confirm();

        salesOrder.Cancel();

        Assert.Equal(SalesOrderStatus.Cancelled, salesOrder.Status);
    }

    [Fact]
    public void Cancel_ChangesStatusToCancelledFromPartiallyFulfilled()
    {
        var salesOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        salesOrder.AddLine(productId, 10, 4.50m);
        salesOrder.Confirm();
        salesOrder.FulfillProduct(productId, 4);

        salesOrder.Cancel();

        Assert.Equal(SalesOrderStatus.Cancelled, salesOrder.Status);
    }

    [Fact]
    public void Cancel_UpdatesUpdatedAt()
    {
        var salesOrder = CreateDraftOrder();
        var originalUpdatedAt = salesOrder.UpdatedAt;

        salesOrder.Cancel();

        Assert.True(salesOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void Cancel_ThrowsWhenOrderIsFullyFulfilled()
    {
        var salesOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        salesOrder.AddLine(productId, 10, 4.50m);
        salesOrder.Confirm();
        salesOrder.FulfillProduct(productId, 10);

        Assert.Throws<DomainException>(() => salesOrder.Cancel());
    }

    [Fact]
    public void Cancel_DoesNotChangeStatusWhenOrderIsFullyFulfilled()
    {
        var salesOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        salesOrder.AddLine(productId, 10, 4.50m);
        salesOrder.Confirm();
        salesOrder.FulfillProduct(productId, 10);

        Assert.Throws<DomainException>(() => salesOrder.Cancel());

        Assert.Equal(SalesOrderStatus.Fulfilled, salesOrder.Status);
    }

    [Fact]
    public void Cancel_ThrowsWhenOrderIsAlreadyCancelled()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.Cancel();

        Assert.Throws<DomainException>(() => salesOrder.Cancel());
    }

    private static SalesOrder CreateDraftOrder()
    {
        return SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
    }
}
