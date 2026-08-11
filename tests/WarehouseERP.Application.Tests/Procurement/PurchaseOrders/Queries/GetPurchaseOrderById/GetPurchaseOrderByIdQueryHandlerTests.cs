using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrderById;
using WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Fakes;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Queries.GetPurchaseOrderById;

public class GetPurchaseOrderByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsPurchaseOrder_WhenItExists()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrderRepository.Seed(purchaseOrder);

        var handler = new GetPurchaseOrderByIdQueryHandler(purchaseOrderRepository);

        var dto = await handler.HandleAsync(new GetPurchaseOrderByIdQuery { Id = purchaseOrder.Id }, CancellationToken.None);

        Assert.Equal(purchaseOrder.Id, dto.Id);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenPurchaseOrderDoesNotExist()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var handler = new GetPurchaseOrderByIdQueryHandler(purchaseOrderRepository);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(
            new GetPurchaseOrderByIdQuery { Id = Guid.NewGuid() }, CancellationToken.None));
    }
}
