using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.SalesOrderLineTests;

public class SalesOrderLineCreateTests
{
    [Fact]
    public void Create_ReturnsLineWithNonEmptyGuid()
    {
        var line = SalesOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);

        Assert.NotEqual(Guid.Empty, line.Id);
    }

    [Fact]
    public void Create_StoresSuppliedValues()
    {
        var salesOrderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var line = SalesOrderLine.Create(salesOrderId, productId, 10, 4.50m);

        Assert.Equal(salesOrderId, line.SalesOrderId);
        Assert.Equal(productId, line.ProductId);
        Assert.Equal(10, line.QuantityOrdered);
        Assert.Equal(4.50m, line.UnitPrice);
    }

    [Fact]
    public void Create_StartsWithNoQuantityFulfilled()
    {
        var line = SalesOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);

        Assert.Equal(0, line.QuantityFulfilled);
    }

    [Fact]
    public void Create_RejectsEmptyProductId()
    {
        Assert.Throws<DomainException>(() => SalesOrderLine.Create(Guid.NewGuid(), Guid.Empty, 10, 4.50m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_RejectsZeroOrNegativeQuantityOrdered(int quantityOrdered)
    {
        Assert.Throws<DomainException>(() => SalesOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), quantityOrdered, 4.50m));
    }

    [Fact]
    public void Create_RejectsNegativeUnitPrice()
    {
        Assert.Throws<DomainException>(() => SalesOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, -0.01m));
    }

    [Fact]
    public void Create_AcceptsZeroUnitPrice()
    {
        var line = SalesOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 0m);

        Assert.Equal(0m, line.UnitPrice);
    }
}
