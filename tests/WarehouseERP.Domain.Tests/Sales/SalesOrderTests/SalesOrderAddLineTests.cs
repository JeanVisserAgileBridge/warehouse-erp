using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.SalesOrderTests;

public class SalesOrderAddLineTests
{
    [Fact]
    public void AddLine_AddsLineWithSuppliedValues()
    {
        var salesOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();

        salesOrder.AddLine(productId, 10, 4.50m);

        var line = Assert.Single(salesOrder.Lines);
        Assert.Equal(salesOrder.Id, line.SalesOrderId);
        Assert.Equal(productId, line.ProductId);
        Assert.Equal(10, line.QuantityOrdered);
        Assert.Equal(0, line.QuantityFulfilled);
        Assert.Equal(4.50m, line.UnitPrice);
    }

    [Fact]
    public void AddLine_UpdatesUpdatedAt()
    {
        var salesOrder = CreateDraftOrder();
        var originalUpdatedAt = salesOrder.UpdatedAt;

        salesOrder.AddLine(Guid.NewGuid(), 10, 4.50m);

        Assert.True(salesOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void AddLine_AllowsMultipleDistinctProducts()
    {
        var salesOrder = CreateDraftOrder();

        salesOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        salesOrder.AddLine(Guid.NewGuid(), 5, 2.00m);

        Assert.Equal(2, salesOrder.Lines.Count);
    }

    [Fact]
    public void AddLine_RejectsDuplicateProductId()
    {
        var salesOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        salesOrder.AddLine(productId, 10, 4.50m);

        Assert.Throws<DomainException>(() => salesOrder.AddLine(productId, 5, 2.00m));
    }

    [Fact]
    public void AddLine_DoesNotAddSecondLineWhenProductIsDuplicate()
    {
        var salesOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        salesOrder.AddLine(productId, 10, 4.50m);

        Assert.Throws<DomainException>(() => salesOrder.AddLine(productId, 5, 2.00m));

        Assert.Single(salesOrder.Lines);
    }

    [Fact]
    public void AddLine_RejectsEmptyProductId()
    {
        var salesOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => salesOrder.AddLine(Guid.Empty, 10, 4.50m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddLine_RejectsZeroOrNegativeQuantityOrdered(int quantityOrdered)
    {
        var salesOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => salesOrder.AddLine(Guid.NewGuid(), quantityOrdered, 4.50m));
    }

    [Fact]
    public void AddLine_RejectsNegativeUnitPrice()
    {
        var salesOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => salesOrder.AddLine(Guid.NewGuid(), 10, -1m));
    }

    [Fact]
    public void AddLine_ThrowsWhenOrderIsConfirmed()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        salesOrder.Confirm();

        Assert.Throws<DomainException>(() => salesOrder.AddLine(Guid.NewGuid(), 5, 2.00m));
    }

    [Fact]
    public void AddLine_ThrowsWhenOrderIsCancelled()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.Cancel();

        Assert.Throws<DomainException>(() => salesOrder.AddLine(Guid.NewGuid(), 5, 2.00m));
    }

    private static SalesOrder CreateDraftOrder()
    {
        return SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
    }
}
