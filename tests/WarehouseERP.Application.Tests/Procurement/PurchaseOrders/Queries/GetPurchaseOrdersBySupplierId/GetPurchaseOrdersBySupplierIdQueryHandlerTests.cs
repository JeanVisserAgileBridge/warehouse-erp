using WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrdersBySupplierId;
using WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Fakes;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Queries.GetPurchaseOrdersBySupplierId;

public class GetPurchaseOrdersBySupplierIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsOnlyPurchaseOrdersForGivenSupplier()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var supplierId = Guid.NewGuid();
        purchaseOrderRepository.Seed(PurchaseOrder.Create(supplierId, "PO-001", DateTime.UtcNow));
        purchaseOrderRepository.Seed(PurchaseOrder.Create(Guid.NewGuid(), "PO-002", DateTime.UtcNow));

        var handler = new GetPurchaseOrdersBySupplierIdQueryHandler(purchaseOrderRepository);

        var result = await handler.HandleAsync(
            new GetPurchaseOrdersBySupplierIdQuery { SupplierId = supplierId }, CancellationToken.None);

        var purchaseOrder = Assert.Single(result);
        Assert.Equal("PO-001", purchaseOrder.OrderNumber);
    }
}
