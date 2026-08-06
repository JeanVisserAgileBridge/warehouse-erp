using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.PurchaseOrderTests;

public class PurchaseOrderSubmitTests
{
    [Fact]
    public void Submit_ChangesStatusToSubmitted()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.AddLine(Guid.NewGuid(), 10, 4.50m);

        purchaseOrder.Submit();

        Assert.Equal(PurchaseOrderStatus.Submitted, purchaseOrder.Status);
    }

    [Fact]
    public void Submit_UpdatesUpdatedAt()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        var originalUpdatedAt = purchaseOrder.UpdatedAt;

        purchaseOrder.Submit();

        Assert.True(purchaseOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void Submit_ThrowsWhenOrderHasNoLines()
    {
        var purchaseOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => purchaseOrder.Submit());
    }

    [Fact]
    public void Submit_DoesNotChangeStatusWhenOrderHasNoLines()
    {
        var purchaseOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => purchaseOrder.Submit());

        Assert.Equal(PurchaseOrderStatus.Draft, purchaseOrder.Status);
    }

    [Fact]
    public void Submit_ThrowsWhenOrderIsAlreadySubmitted()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        purchaseOrder.Submit();

        Assert.Throws<DomainException>(() => purchaseOrder.Submit());
    }

    [Fact]
    public void Submit_ThrowsWhenOrderIsCancelled()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.Cancel();

        Assert.Throws<DomainException>(() => purchaseOrder.Submit());
    }

    private static PurchaseOrder CreateDraftOrder()
    {
        return PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
    }
}
