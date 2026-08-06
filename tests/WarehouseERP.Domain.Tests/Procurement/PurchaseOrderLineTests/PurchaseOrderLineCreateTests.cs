using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.PurchaseOrderLineTests;

public class PurchaseOrderLineCreateTests
{
    [Fact]
    public void Create_ReturnsLineWithNonEmptyGuid()
    {
        var line = PurchaseOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);

        Assert.NotEqual(Guid.Empty, line.Id);
    }

    [Fact]
    public void Create_StoresSuppliedValues()
    {
        var purchaseOrderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var line = PurchaseOrderLine.Create(purchaseOrderId, productId, 10, 4.50m);

        Assert.Equal(purchaseOrderId, line.PurchaseOrderId);
        Assert.Equal(productId, line.ProductId);
        Assert.Equal(10, line.QuantityOrdered);
        Assert.Equal(4.50m, line.UnitPrice);
    }

    [Fact]
    public void Create_StartsWithNoQuantityReceived()
    {
        var line = PurchaseOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);

        Assert.Equal(0, line.QuantityReceived);
    }

    [Fact]
    public void Create_RejectsEmptyProductId()
    {
        Assert.Throws<DomainException>(() => PurchaseOrderLine.Create(Guid.NewGuid(), Guid.Empty, 10, 4.50m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_RejectsZeroOrNegativeQuantityOrdered(int quantityOrdered)
    {
        Assert.Throws<DomainException>(() => PurchaseOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), quantityOrdered, 4.50m));
    }

    [Fact]
    public void Create_RejectsNegativeUnitPrice()
    {
        Assert.Throws<DomainException>(() => PurchaseOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, -0.01m));
    }

    [Fact]
    public void Create_AcceptsZeroUnitPrice()
    {
        var line = PurchaseOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 0m);

        Assert.Equal(0m, line.UnitPrice);
    }
}
