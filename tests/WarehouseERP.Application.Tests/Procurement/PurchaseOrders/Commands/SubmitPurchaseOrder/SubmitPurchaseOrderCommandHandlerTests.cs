using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.SubmitPurchaseOrder;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Commands.SubmitPurchaseOrder;

public class SubmitPurchaseOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_SubmitsOrder_WhenDraftWithLines()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrder.AddLine(Guid.NewGuid(), 10, 5.00m);
        purchaseOrderRepository.Seed(purchaseOrder);

        var handler = new SubmitPurchaseOrderCommandHandler(purchaseOrderRepository, unitOfWork);

        var dto = await handler.HandleAsync(
            new SubmitPurchaseOrderCommand { PurchaseOrderId = purchaseOrder.Id }, CancellationToken.None);

        Assert.Equal(PurchaseOrderStatus.Submitted, dto.Status);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenPurchaseOrderDoesNotExist()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new SubmitPurchaseOrderCommandHandler(purchaseOrderRepository, unitOfWork);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(
            new SubmitPurchaseOrderCommand { PurchaseOrderId = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenOrderHasNoLines()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrderRepository.Seed(purchaseOrder);

        var handler = new SubmitPurchaseOrderCommandHandler(purchaseOrderRepository, unitOfWork);

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(
            new SubmitPurchaseOrderCommand { PurchaseOrderId = purchaseOrder.Id }, CancellationToken.None));
    }
}
