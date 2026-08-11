using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.UpdatePurchaseOrderLine;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Commands.UpdatePurchaseOrderLine;

public class UpdatePurchaseOrderLineCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_UpdatesQuantityAndPrice_WhenOrderIsDraft()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var productId = Guid.NewGuid();
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrder.AddLine(productId, 10, 5.00m);
        purchaseOrderRepository.Seed(purchaseOrder);

        var handler = new UpdatePurchaseOrderLineCommandHandler(purchaseOrderRepository, unitOfWork);

        var command = new UpdatePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = productId,
            QuantityOrdered = 20,
            UnitPrice = 6.50m
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        var line = Assert.Single(dto.Lines);
        Assert.Equal(20, line.QuantityOrdered);
        Assert.Equal(6.50m, line.UnitPrice);
        Assert.Equal(0, line.QuantityReceived);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenPurchaseOrderDoesNotExist()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new UpdatePurchaseOrderLineCommandHandler(purchaseOrderRepository, unitOfWork);

        var command = new UpdatePurchaseOrderLineCommand
        {
            PurchaseOrderId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            QuantityOrdered = 5,
            UnitPrice = 1.00m
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
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

        var handler = new UpdatePurchaseOrderLineCommandHandler(purchaseOrderRepository, unitOfWork);

        var command = new UpdatePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = productId,
            QuantityOrdered = 20,
            UnitPrice = 6.50m
        };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenProductIsNotOnOrder()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrderRepository.Seed(purchaseOrder);

        var handler = new UpdatePurchaseOrderLineCommandHandler(purchaseOrderRepository, unitOfWork);

        var command = new UpdatePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = Guid.NewGuid(),
            QuantityOrdered = 20,
            UnitPrice = 6.50m
        };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));
    }
}
