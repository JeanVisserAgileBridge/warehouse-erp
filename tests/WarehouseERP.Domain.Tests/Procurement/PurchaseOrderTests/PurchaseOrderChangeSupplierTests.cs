using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.PurchaseOrderTests;

public class PurchaseOrderChangeSupplierTests
{
    [Fact]
    public void ChangeSupplier_UpdatesSupplierId()
    {
        var purchaseOrder = CreateDraftOrder();
        var newSupplierId = Guid.NewGuid();

        purchaseOrder.ChangeSupplier(newSupplierId);

        Assert.Equal(newSupplierId, purchaseOrder.SupplierId);
    }

    [Fact]
    public void ChangeSupplier_UpdatesUpdatedAt()
    {
        var purchaseOrder = CreateDraftOrder();
        var originalUpdatedAt = purchaseOrder.UpdatedAt;

        purchaseOrder.ChangeSupplier(Guid.NewGuid());

        Assert.True(purchaseOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ChangeSupplier_RejectsEmptySupplierId()
    {
        var purchaseOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => purchaseOrder.ChangeSupplier(Guid.Empty));
    }

    [Fact]
    public void ChangeSupplier_ThrowsWhenOrderIsSubmitted()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        purchaseOrder.Submit();

        Assert.Throws<DomainException>(() => purchaseOrder.ChangeSupplier(Guid.NewGuid()));
    }

    [Fact]
    public void ChangeSupplier_ThrowsWhenOrderIsCancelled()
    {
        var purchaseOrder = CreateDraftOrder();
        purchaseOrder.Cancel();

        Assert.Throws<DomainException>(() => purchaseOrder.ChangeSupplier(Guid.NewGuid()));
    }

    private static PurchaseOrder CreateDraftOrder()
    {
        return PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
    }
}
