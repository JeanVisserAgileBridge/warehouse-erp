using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.PurchaseOrderTests;

public class PurchaseOrderCancelTests
{
    [Fact]
    public void Cancel_ChangesStatusToCancelledFromDraft()
    {
        var purchaseOrder = CreateDraftOrder();

        purchaseOrder.Cancel();

        Assert.Equal(PurchaseOrderStatus.Cancelled, purchaseOrder.Status);
    }

    [Fact]
    public void Cancel_ChangesStatusToCancelledFromSubmitted()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        purchaseOrder.Submit();

        purchaseOrder.Cancel();

        Assert.Equal(PurchaseOrderStatus.Cancelled, purchaseOrder.Status);
    }

    [Fact]
    public void Cancel_ChangesStatusToCancelledFromPartiallyReceived()
    {
        var purchaseOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        purchaseOrder.AddLine(productId, 10, 4.50m);
        purchaseOrder.Submit();
        purchaseOrder.ReceiveProduct(productId, 4);

        purchaseOrder.Cancel();

        Assert.Equal(PurchaseOrderStatus.Cancelled, purchaseOrder.Status);
    }

    [Fact]
    public void Cancel_UpdatesUpdatedAt()
    {
        var purchaseOrder = CreateDraftOrder();
        var originalUpdatedAt = purchaseOrder.UpdatedAt;

        purchaseOrder.Cancel();

        Assert.True(purchaseOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void Cancel_ThrowsWhenOrderIsFullyReceived()
    {
        var purchaseOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        purchaseOrder.AddLine(productId, 10, 4.50m);
        purchaseOrder.Submit();
        purchaseOrder.ReceiveProduct(productId, 10);

        Assert.Throws<DomainException>(() => purchaseOrder.Cancel());
    }

    [Fact]
    public void Cancel_DoesNotChangeStatusWhenOrderIsFullyReceived()
    {
        var purchaseOrder = CreateDraftOrder();
        var productId = Guid.NewGuid();
        purchaseOrder.AddLine(productId, 10, 4.50m);
        purchaseOrder.Submit();
        purchaseOrder.ReceiveProduct(productId, 10);

        Assert.Throws<DomainException>(() => purchaseOrder.Cancel());

        Assert.Equal(PurchaseOrderStatus.Received, purchaseOrder.Status);
    }

    [Fact]
    public void Cancel_ThrowsWhenOrderIsAlreadyCancelled()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.Cancel();

        Assert.Throws<DomainException>(() => purchaseOrder.Cancel());
    }

    private static PurchaseOrder CreateDraftOrder()
    {
        return PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
    }
}
