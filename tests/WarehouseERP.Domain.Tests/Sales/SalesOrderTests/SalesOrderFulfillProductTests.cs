using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.SalesOrderTests;

public class SalesOrderFulfillProductTests
{
    [Fact]
    public void FulfillProduct_IncreasesQuantityFulfilledOnMatchingLine()
    {
        var salesOrder = CreateConfirmedOrder(out var productId, quantityOrdered: 10);

        salesOrder.FulfillProduct(productId, 4);

        var line = Assert.Single(salesOrder.Lines);
        Assert.Equal(4, line.QuantityFulfilled);
    }

    [Fact]
    public void FulfillProduct_AccumulatesAcrossMultipleCalls()
    {
        var salesOrder = CreateConfirmedOrder(out var productId, quantityOrdered: 10);

        salesOrder.FulfillProduct(productId, 4);
        salesOrder.FulfillProduct(productId, 3);

        var line = Assert.Single(salesOrder.Lines);
        Assert.Equal(7, line.QuantityFulfilled);
    }

    [Fact]
    public void FulfillProduct_SetsStatusToPartiallyFulfilledWhenSomeQuantityRemains()
    {
        var salesOrder = CreateConfirmedOrder(out var productId, quantityOrdered: 10);

        salesOrder.FulfillProduct(productId, 4);

        Assert.Equal(SalesOrderStatus.PartiallyFulfilled, salesOrder.Status);
    }

    [Fact]
    public void FulfillProduct_SetsStatusToFulfilledWhenLineIsFullyFulfilled()
    {
        var salesOrder = CreateConfirmedOrder(out var productId, quantityOrdered: 10);

        salesOrder.FulfillProduct(productId, 10);

        Assert.Equal(SalesOrderStatus.Fulfilled, salesOrder.Status);
    }

    [Fact]
    public void FulfillProduct_SetsStatusToFulfilledWhenFinalPartialCallCompletesLine()
    {
        var salesOrder = CreateConfirmedOrder(out var productId, quantityOrdered: 10);
        salesOrder.FulfillProduct(productId, 4);

        salesOrder.FulfillProduct(productId, 6);

        Assert.Equal(SalesOrderStatus.Fulfilled, salesOrder.Status);
    }

    [Fact]
    public void FulfillProduct_KeepsStatusPartiallyFulfilledWhenOneOfMultipleLinesIsFullyFulfilled()
    {
        var salesOrder = CreateDraftOrder();
        var fulfilledProductId = Guid.NewGuid();
        var pendingProductId = Guid.NewGuid();
        salesOrder.AddLine(fulfilledProductId, 10, 4.50m);
        salesOrder.AddLine(pendingProductId, 5, 2.00m);
        salesOrder.Confirm();

        salesOrder.FulfillProduct(fulfilledProductId, 10);

        Assert.Equal(SalesOrderStatus.PartiallyFulfilled, salesOrder.Status);
    }

    [Fact]
    public void FulfillProduct_SetsStatusToFulfilledOnlyWhenAllLinesAreFullyFulfilled()
    {
        var salesOrder = CreateDraftOrder();
        var firstProductId = Guid.NewGuid();
        var secondProductId = Guid.NewGuid();
        salesOrder.AddLine(firstProductId, 10, 4.50m);
        salesOrder.AddLine(secondProductId, 5, 2.00m);
        salesOrder.Confirm();
        salesOrder.FulfillProduct(firstProductId, 10);

        salesOrder.FulfillProduct(secondProductId, 5);

        Assert.Equal(SalesOrderStatus.Fulfilled, salesOrder.Status);
    }

    [Fact]
    public void FulfillProduct_UpdatesUpdatedAt()
    {
        var salesOrder = CreateConfirmedOrder(out var productId, quantityOrdered: 10);
        var originalUpdatedAt = salesOrder.UpdatedAt;

        salesOrder.FulfillProduct(productId, 4);

        Assert.True(salesOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void FulfillProduct_AllowsFulfillingWhilePartiallyFulfilled()
    {
        var salesOrder = CreateConfirmedOrder(out var productId, quantityOrdered: 10);
        salesOrder.FulfillProduct(productId, 4);

        salesOrder.FulfillProduct(productId, 6);

        var line = Assert.Single(salesOrder.Lines);
        Assert.Equal(10, line.QuantityFulfilled);
    }

    [Fact]
    public void FulfillProduct_ThrowsWhenTotalFulfilledWouldExceedQuantityOrdered()
    {
        var salesOrder = CreateConfirmedOrder(out var productId, quantityOrdered: 10);
        salesOrder.FulfillProduct(productId, 8);

        Assert.Throws<DomainException>(() => salesOrder.FulfillProduct(productId, 3));
    }

    [Fact]
    public void FulfillProduct_ThrowsWhenProductIsNotOnOrder()
    {
        var salesOrder = CreateConfirmedOrder(out _, quantityOrdered: 10);

        Assert.Throws<DomainException>(() => salesOrder.FulfillProduct(Guid.NewGuid(), 1));
    }

    [Fact]
    public void FulfillProduct_ThrowsWhenOrderIsStillDraft()
    {
        var salesOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        salesOrder.AddLine(productId, 10, 4.50m);

        Assert.Throws<DomainException>(() => salesOrder.FulfillProduct(productId, 4));
    }

    [Fact]
    public void FulfillProduct_ThrowsWhenOrderIsCancelled()
    {
        var salesOrder = CreateConfirmedOrder(out var productId, quantityOrdered: 10);
        salesOrder.Cancel();

        Assert.Throws<DomainException>(() => salesOrder.FulfillProduct(productId, 4));
    }

    [Fact]
    public void FulfillProduct_ThrowsWhenOrderIsAlreadyFullyFulfilled()
    {
        var salesOrder = CreateConfirmedOrder(out var productId, quantityOrdered: 10);
        salesOrder.FulfillProduct(productId, 10);

        Assert.Throws<DomainException>(() => salesOrder.FulfillProduct(productId, 1));
    }

    private static SalesOrder CreateDraftOrder()
    {
        return SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
    }

    private static SalesOrder CreateConfirmedOrder(out Guid productId, int quantityOrdered)
    {
        var salesOrder = CreateDraftOrder();
        productId = Guid.NewGuid();
        salesOrder.AddLine(productId, quantityOrdered, 4.50m);
        salesOrder.Confirm();

        return salesOrder;
    }
}
