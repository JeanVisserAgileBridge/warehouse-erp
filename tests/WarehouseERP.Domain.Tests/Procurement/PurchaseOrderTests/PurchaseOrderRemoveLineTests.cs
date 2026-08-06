using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.PurchaseOrderTests;

public class PurchaseOrderRemoveLineTests
{
    [Fact]
    public void RemoveLine_RemovesMatchingLine()
    {
        var purchaseOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        purchaseOrder.AddLine(productId, 10, 4.50m);

        purchaseOrder.RemoveLine(productId);

        Assert.Empty(purchaseOrder.Lines);
    }

    [Fact]
    public void RemoveLine_UpdatesUpdatedAt()
    {
        var purchaseOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        purchaseOrder.AddLine(productId, 10, 4.50m);
        var originalUpdatedAt = purchaseOrder.UpdatedAt;

        purchaseOrder.RemoveLine(productId);

        Assert.True(purchaseOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void RemoveLine_LeavesOtherLinesIntact()
    {
        var purchaseOrder = CreateDraftOrder();
        var productIdToRemove = Guid.NewGuid();
        var productIdToKeep = Guid.NewGuid();
        purchaseOrder.AddLine(productIdToRemove, 10, 4.50m);
        purchaseOrder.AddLine(productIdToKeep, 5, 2.00m);

        purchaseOrder.RemoveLine(productIdToRemove);

        var remainingLine = Assert.Single(purchaseOrder.Lines);
        Assert.Equal(productIdToKeep, remainingLine.ProductId);
    }

    [Fact]
    public void RemoveLine_AllowsReAddingSameProductAfterRemoval()
    {
        var purchaseOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        purchaseOrder.AddLine(productId, 10, 4.50m);
        purchaseOrder.RemoveLine(productId);

        purchaseOrder.AddLine(productId, 5, 2.00m);

        var line = Assert.Single(purchaseOrder.Lines);
        Assert.Equal(5, line.QuantityOrdered);
    }

    [Fact]
    public void RemoveLine_ThrowsWhenProductIsNotOnOrder()
    {
        var purchaseOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => purchaseOrder.RemoveLine(Guid.NewGuid()));
    }

    [Fact]
    public void RemoveLine_ThrowsWhenOrderIsSubmitted()
    {
        var purchaseOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        purchaseOrder.AddLine(productId, 10, 4.50m);
        purchaseOrder.Submit();

        Assert.Throws<DomainException>(() => purchaseOrder.RemoveLine(productId));
    }

    [Fact]
    public void RemoveLine_ThrowsWhenOrderIsCancelled()
    {
        var purchaseOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        purchaseOrder.AddLine(productId, 10, 4.50m);
        purchaseOrder.Cancel();

        Assert.Throws<DomainException>(() => purchaseOrder.RemoveLine(productId));
    }

    private static PurchaseOrder CreateDraftOrder()
    {
        return PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
    }
}
