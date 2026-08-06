using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.PurchaseOrderTests;

public class PurchaseOrderReceiveProductTests
{
    [Fact]
    public void ReceiveProduct_IncreasesQuantityReceivedOnMatchingLine()
    {
        var purchaseOrder = CreateSubmittedOrder(out var productId, quantityOrdered: 10);

        purchaseOrder.ReceiveProduct(productId, 4);

        var line = Assert.Single(purchaseOrder.Lines);
        Assert.Equal(4, line.QuantityReceived);
    }

    [Fact]
    public void ReceiveProduct_AccumulatesAcrossMultipleCalls()
    {
        var purchaseOrder = CreateSubmittedOrder(out var productId, quantityOrdered: 10);

        purchaseOrder.ReceiveProduct(productId, 4);
        purchaseOrder.ReceiveProduct(productId, 3);

        var line = Assert.Single(purchaseOrder.Lines);
        Assert.Equal(7, line.QuantityReceived);
    }

    [Fact]
    public void ReceiveProduct_SetsStatusToPartiallyReceivedWhenSomeQuantityRemains()
    {
        var purchaseOrder = CreateSubmittedOrder(out var productId, quantityOrdered: 10);

        purchaseOrder.ReceiveProduct(productId, 4);

        Assert.Equal(PurchaseOrderStatus.PartiallyReceived, purchaseOrder.Status);
    }

    [Fact]
    public void ReceiveProduct_SetsStatusToReceivedWhenLineIsFullyReceived()
    {
        var purchaseOrder = CreateSubmittedOrder(out var productId, quantityOrdered: 10);

        purchaseOrder.ReceiveProduct(productId, 10);

        Assert.Equal(PurchaseOrderStatus.Received, purchaseOrder.Status);
    }

    [Fact]
    public void ReceiveProduct_SetsStatusToReceivedWhenFinalPartialCallCompletesLine()
    {
        var purchaseOrder = CreateSubmittedOrder(out var productId, quantityOrdered: 10);
        purchaseOrder.ReceiveProduct(productId, 4);

        purchaseOrder.ReceiveProduct(productId, 6);

        Assert.Equal(PurchaseOrderStatus.Received, purchaseOrder.Status);
    }

    [Fact]
    public void ReceiveProduct_KeepsStatusPartiallyReceivedWhenOneOfMultipleLinesIsFullyReceived()
    {
        var purchaseOrder = CreateDraftOrder();
        var receivedProductId = Guid.NewGuid();
        var pendingProductId = Guid.NewGuid();
        purchaseOrder.AddLine(receivedProductId, 10, 4.50m);
        purchaseOrder.AddLine(pendingProductId, 5, 2.00m);
        purchaseOrder.Submit();

        purchaseOrder.ReceiveProduct(receivedProductId, 10);

        Assert.Equal(PurchaseOrderStatus.PartiallyReceived, purchaseOrder.Status);
    }

    [Fact]
    public void ReceiveProduct_SetsStatusToReceivedOnlyWhenAllLinesAreFullyReceived()
    {
        var purchaseOrder = CreateDraftOrder();
        var firstProductId = Guid.NewGuid();
        var secondProductId = Guid.NewGuid();
        purchaseOrder.AddLine(firstProductId, 10, 4.50m);
        purchaseOrder.AddLine(secondProductId, 5, 2.00m);
        purchaseOrder.Submit();
        purchaseOrder.ReceiveProduct(firstProductId, 10);

        purchaseOrder.ReceiveProduct(secondProductId, 5);

        Assert.Equal(PurchaseOrderStatus.Received, purchaseOrder.Status);
    }

    [Fact]
    public void ReceiveProduct_UpdatesUpdatedAt()
    {
        var purchaseOrder = CreateSubmittedOrder(out var productId, quantityOrdered: 10);
        var originalUpdatedAt = purchaseOrder.UpdatedAt;

        purchaseOrder.ReceiveProduct(productId, 4);

        Assert.True(purchaseOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ReceiveProduct_AllowsReceivingWhilePartiallyReceived()
    {
        var purchaseOrder = CreateSubmittedOrder(out var productId, quantityOrdered: 10);
        purchaseOrder.ReceiveProduct(productId, 4);

        purchaseOrder.ReceiveProduct(productId, 6);

        var line = Assert.Single(purchaseOrder.Lines);
        Assert.Equal(10, line.QuantityReceived);
    }

    [Fact]
    public void ReceiveProduct_ThrowsWhenTotalReceivedWouldExceedQuantityOrdered()
    {
        var purchaseOrder = CreateSubmittedOrder(out var productId, quantityOrdered: 10);
        purchaseOrder.ReceiveProduct(productId, 8);

        Assert.Throws<DomainException>(() => purchaseOrder.ReceiveProduct(productId, 3));
    }

    [Fact]
    public void ReceiveProduct_ThrowsWhenProductIsNotOnOrder()
    {
        var purchaseOrder = CreateSubmittedOrder(out _, quantityOrdered: 10);

        Assert.Throws<DomainException>(() => purchaseOrder.ReceiveProduct(Guid.NewGuid(), 1));
    }

    [Fact]
    public void ReceiveProduct_ThrowsWhenOrderIsStillDraft()
    {
        var purchaseOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        purchaseOrder.AddLine(productId, 10, 4.50m);

        Assert.Throws<DomainException>(() => purchaseOrder.ReceiveProduct(productId, 4));
    }

    [Fact]
    public void ReceiveProduct_ThrowsWhenOrderIsCancelled()
    {
        var purchaseOrder = CreateSubmittedOrder(out var productId, quantityOrdered: 10);
        purchaseOrder.Cancel();

        Assert.Throws<DomainException>(() => purchaseOrder.ReceiveProduct(productId, 4));
    }

    [Fact]
    public void ReceiveProduct_ThrowsWhenOrderIsAlreadyFullyReceived()
    {
        var purchaseOrder = CreateSubmittedOrder(out var productId, quantityOrdered: 10);
        purchaseOrder.ReceiveProduct(productId, 10);

        Assert.Throws<DomainException>(() => purchaseOrder.ReceiveProduct(productId, 1));
    }

    private static PurchaseOrder CreateDraftOrder()
    {
        return PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
    }

    private static PurchaseOrder CreateSubmittedOrder(out Guid productId, int quantityOrdered)
    {
        var purchaseOrder = CreateDraftOrder();
        productId = Guid.NewGuid();
        purchaseOrder.AddLine(productId, quantityOrdered, 4.50m);
        purchaseOrder.Submit();

        return purchaseOrder;
    }
}
