using WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrders;
using WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Fakes;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Queries.GetPurchaseOrders;

public class GetPurchaseOrdersQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsAllPurchaseOrders()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        purchaseOrderRepository.Seed(PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow));
        purchaseOrderRepository.Seed(PurchaseOrder.Create(Guid.NewGuid(), "PO-002", DateTime.UtcNow));

        var handler = new GetPurchaseOrdersQueryHandler(purchaseOrderRepository);

        var result = await handler.HandleAsync(new GetPurchaseOrdersQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
    }
}
