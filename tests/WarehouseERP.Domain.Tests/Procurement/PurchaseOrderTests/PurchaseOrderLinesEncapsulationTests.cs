using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.PurchaseOrderTests;

public class PurchaseOrderLinesEncapsulationTests
{
    [Fact]
    public void Lines_IsExposedAsReadOnlyCollection()
    {
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrder.AddLine(Guid.NewGuid(), 10, 4.50m);

        Assert.IsAssignableFrom<IReadOnlyCollection<PurchaseOrderLine>>(purchaseOrder.Lines);
    }

    [Fact]
    public void Lines_ThrowsWhenCallerAttemptsToMutateTheUnderlyingCollection()
    {
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrder.AddLine(Guid.NewGuid(), 10, 4.50m);

        var mutableView = Assert.IsAssignableFrom<ICollection<PurchaseOrderLine>>(purchaseOrder.Lines);
        var extraLine = PurchaseOrderLine.Create(purchaseOrder.Id, Guid.NewGuid(), 1, 1m);

        Assert.Throws<NotSupportedException>(() => mutableView.Add(extraLine));
        Assert.Single(purchaseOrder.Lines);
    }
}
