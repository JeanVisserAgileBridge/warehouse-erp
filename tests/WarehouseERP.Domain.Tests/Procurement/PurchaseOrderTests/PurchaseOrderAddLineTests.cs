using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.PurchaseOrderTests;

public class PurchaseOrderAddLineTests
{
    [Fact]
    public void AddLine_AddsLineWithSuppliedValues()
    {
        var purchaseOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();

        purchaseOrder.AddLine(productId, 10, 4.50m);

        var line = Assert.Single(purchaseOrder.Lines);
        Assert.Equal(purchaseOrder.Id, line.PurchaseOrderId);
        Assert.Equal(productId, line.ProductId);
        Assert.Equal(10, line.QuantityOrdered);
        Assert.Equal(0, line.QuantityReceived);
        Assert.Equal(4.50m, line.UnitPrice);
    }

    [Fact]
    public void AddLine_UpdatesUpdatedAt()
    {
        var purchaseOrder = CreateDraftOrder();
        var originalUpdatedAt = purchaseOrder.UpdatedAt;

        purchaseOrder.AddLine(Guid.NewGuid(), 10, 4.50m);

        Assert.True(purchaseOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void AddLine_AllowsMultipleDistinctProducts()
    {
        var purchaseOrder = CreateDraftOrder();

        purchaseOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        purchaseOrder.AddLine(Guid.NewGuid(), 5, 2.00m);

        Assert.Equal(2, purchaseOrder.Lines.Count);
    }

    [Fact]
    public void AddLine_RejectsDuplicateProductId()
    {
        var purchaseOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        purchaseOrder.AddLine(productId, 10, 4.50m);

        Assert.Throws<DomainException>(() => purchaseOrder.AddLine(productId, 5, 2.00m));
    }

    [Fact]
    public void AddLine_DoesNotAddSecondLineWhenProductIsDuplicate()
    {
        var purchaseOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        purchaseOrder.AddLine(productId, 10, 4.50m);

        Assert.Throws<DomainException>(() => purchaseOrder.AddLine(productId, 5, 2.00m));

        Assert.Single(purchaseOrder.Lines);
    }

    [Fact]
    public void AddLine_RejectsEmptyProductId()
    {
        var purchaseOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => purchaseOrder.AddLine(Guid.Empty, 10, 4.50m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddLine_RejectsZeroOrNegativeQuantityOrdered(int quantityOrdered)
    {
        var purchaseOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => purchaseOrder.AddLine(Guid.NewGuid(), quantityOrdered, 4.50m));
    }

    [Fact]
    public void AddLine_RejectsNegativeUnitPrice()
    {
        var purchaseOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => purchaseOrder.AddLine(Guid.NewGuid(), 10, -1m));
    }

    [Fact]
    public void AddLine_ThrowsWhenOrderIsSubmitted()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        purchaseOrder.Submit();

        Assert.Throws<DomainException>(() => purchaseOrder.AddLine(Guid.NewGuid(), 5, 2.00m));
    }

    [Fact]
    public void AddLine_ThrowsWhenOrderIsCancelled()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.Cancel();

        Assert.Throws<DomainException>(() => purchaseOrder.AddLine(Guid.NewGuid(), 5, 2.00m));
    }

    private static PurchaseOrder CreateDraftOrder()
    {
        return PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
    }
}
