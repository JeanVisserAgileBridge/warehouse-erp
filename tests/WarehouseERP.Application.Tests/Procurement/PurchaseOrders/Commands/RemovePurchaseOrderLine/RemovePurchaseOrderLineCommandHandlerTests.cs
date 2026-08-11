using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.RemovePurchaseOrderLine;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Commands.RemovePurchaseOrderLine;

public class RemovePurchaseOrderLineCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_RemovesLine_WhenOrderIsDraft()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var productId = Guid.NewGuid();
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrder.AddLine(productId, 10, 5.00m);
        purchaseOrderRepository.Seed(purchaseOrder);

        var handler = new RemovePurchaseOrderLineCommandHandler(purchaseOrderRepository, unitOfWork);

        var dto = await handler.HandleAsync(
            new RemovePurchaseOrderLineCommand { PurchaseOrderId = purchaseOrder.Id, ProductId = productId },
            CancellationToken.None);

        Assert.Empty(dto.Lines);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenPurchaseOrderDoesNotExist()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new RemovePurchaseOrderLineCommandHandler(purchaseOrderRepository, unitOfWork);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(
            new RemovePurchaseOrderLineCommand { PurchaseOrderId = Guid.NewGuid(), ProductId = Guid.NewGuid() },
            CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenOrderIsNotDraft()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var productId = Guid.NewGuid();
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrder.AddLine(productId, 10, 5.00m);
        purchaseOrder.Submit();
        purchaseOrderRepository.Seed(purchaseOrder);

        var handler = new RemovePurchaseOrderLineCommandHandler(purchaseOrderRepository, unitOfWork);

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(
            new RemovePurchaseOrderLineCommand { PurchaseOrderId = purchaseOrder.Id, ProductId = productId },
            CancellationToken.None));
    }
}
