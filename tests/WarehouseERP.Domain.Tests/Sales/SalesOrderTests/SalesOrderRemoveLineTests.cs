using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.SalesOrderTests;

public class SalesOrderRemoveLineTests
{
    [Fact]
    public void RemoveLine_RemovesMatchingLine()
    {
        var salesOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        salesOrder.AddLine(productId, 10, 4.50m);

        salesOrder.RemoveLine(productId);

        Assert.Empty(salesOrder.Lines);
    }

    [Fact]
    public void RemoveLine_UpdatesUpdatedAt()
    {
        var salesOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        salesOrder.AddLine(productId, 10, 4.50m);
        var originalUpdatedAt = salesOrder.UpdatedAt;

        salesOrder.RemoveLine(productId);

        Assert.True(salesOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void RemoveLine_LeavesOtherLinesIntact()
    {
        var salesOrder = CreateDraftOrder();
        var productIdToRemove = Guid.NewGuid();
        var productIdToKeep = Guid.NewGuid();
        salesOrder.AddLine(productIdToRemove, 10, 4.50m);
        salesOrder.AddLine(productIdToKeep, 5, 2.00m);

        salesOrder.RemoveLine(productIdToRemove);

        var remainingLine = Assert.Single(salesOrder.Lines);
        Assert.Equal(productIdToKeep, remainingLine.ProductId);
    }

    [Fact]
    public void RemoveLine_AllowsReAddingSameProductAfterRemoval()
    {
        var salesOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        salesOrder.AddLine(productId, 10, 4.50m);
        salesOrder.RemoveLine(productId);

        salesOrder.AddLine(productId, 5, 2.00m);

        var line = Assert.Single(salesOrder.Lines);
        Assert.Equal(5, line.QuantityOrdered);
    }

    [Fact]
    public void RemoveLine_ThrowsWhenProductIsNotOnOrder()
    {
        var salesOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => salesOrder.RemoveLine(Guid.NewGuid()));
    }

    [Fact]
    public void RemoveLine_ThrowsWhenOrderIsConfirmed()
    {
        var salesOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        salesOrder.AddLine(productId, 10, 4.50m);
        salesOrder.Confirm();

        Assert.Throws<DomainException>(() => salesOrder.RemoveLine(productId));
    }

    [Fact]
    public void RemoveLine_ThrowsWhenOrderIsCancelled()
    {
        var salesOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        salesOrder.AddLine(productId, 10, 4.50m);
        salesOrder.Cancel();

        Assert.Throws<DomainException>(() => salesOrder.RemoveLine(productId));
    }

    private static SalesOrder CreateDraftOrder()
    {
        return SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
    }
}
