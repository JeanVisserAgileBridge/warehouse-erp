using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.CancelPurchaseOrder;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Commands.CancelPurchaseOrder;

public class CancelPurchaseOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_CancelsOrder_WhenNotYetReceived()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrder.AddLine(Guid.NewGuid(), 10, 5.00m);
        purchaseOrder.Submit();
        purchaseOrderRepository.Seed(purchaseOrder);

        var handler = new CancelPurchaseOrderCommandHandler(purchaseOrderRepository, unitOfWork);

        var dto = await handler.HandleAsync(
            new CancelPurchaseOrderCommand { PurchaseOrderId = purchaseOrder.Id }, CancellationToken.None);

        Assert.Equal(PurchaseOrderStatus.Cancelled, dto.Status);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenPurchaseOrderDoesNotExist()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new CancelPurchaseOrderCommandHandler(purchaseOrderRepository, unitOfWork);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(
            new CancelPurchaseOrderCommand { PurchaseOrderId = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenOrderIsFullyReceived()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var productId = Guid.NewGuid();
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrder.AddLine(productId, 10, 5.00m);
        purchaseOrder.Submit();
        purchaseOrder.ReceiveProduct(productId, 10);
        purchaseOrderRepository.Seed(purchaseOrder);

        var handler = new CancelPurchaseOrderCommandHandler(purchaseOrderRepository, unitOfWork);

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(
            new CancelPurchaseOrderCommand { PurchaseOrderId = purchaseOrder.Id }, CancellationToken.None));
    }
}
