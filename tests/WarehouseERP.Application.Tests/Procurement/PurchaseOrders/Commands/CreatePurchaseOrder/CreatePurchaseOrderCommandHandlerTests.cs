using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.CreatePurchaseOrder;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Fakes;
using WarehouseERP.Application.Tests.Procurement.Suppliers.Fakes;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Commands.CreatePurchaseOrder;

public class CreatePurchaseOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_CreatesPurchaseOrder_WhenSupplierExistsAndIsActive()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var supplierRepository = new FakeSupplierRepository();
        var unitOfWork = new FakeUnitOfWork();

        var supplier = Supplier.Create("Acme Supplies");
        supplierRepository.Seed(supplier);

        var handler = new CreatePurchaseOrderCommandHandler(purchaseOrderRepository, supplierRepository, unitOfWork);

        var command = new CreatePurchaseOrderCommand
        {
            SupplierId = supplier.Id,
            OrderNumber = "PO-001",
            OrderDate = DateTime.UtcNow
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("PO-001", dto.OrderNumber);
        Assert.Equal(PurchaseOrderStatus.Draft, dto.Status);
        Assert.Equal(1, purchaseOrderRepository.AddCallCount);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenSupplierDoesNotExist()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var supplierRepository = new FakeSupplierRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new CreatePurchaseOrderCommandHandler(purchaseOrderRepository, supplierRepository, unitOfWork);

        var command = new CreatePurchaseOrderCommand
        {
            SupplierId = Guid.NewGuid(),
            OrderNumber = "PO-001",
            OrderDate = DateTime.UtcNow
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsInactiveSupplierException_WhenSupplierIsNotActive()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var supplierRepository = new FakeSupplierRepository();
        var unitOfWork = new FakeUnitOfWork();

        var supplier = Supplier.Create("Acme Supplies");
        supplier.Deactivate();
        supplierRepository.Seed(supplier);

        var handler = new CreatePurchaseOrderCommandHandler(purchaseOrderRepository, supplierRepository, unitOfWork);

        var command = new CreatePurchaseOrderCommand
        {
            SupplierId = supplier.Id,
            OrderNumber = "PO-001",
            OrderDate = DateTime.UtcNow
        };

        await Assert.ThrowsAsync<InactiveSupplierException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateOrderNumberException_WhenOrderNumberAlreadyExists()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var supplierRepository = new FakeSupplierRepository();
        var unitOfWork = new FakeUnitOfWork();

        var supplier = Supplier.Create("Acme Supplies");
        supplierRepository.Seed(supplier);
        purchaseOrderRepository.Seed(PurchaseOrder.Create(supplier.Id, "PO-001", DateTime.UtcNow));

        var handler = new CreatePurchaseOrderCommandHandler(purchaseOrderRepository, supplierRepository, unitOfWork);

        var command = new CreatePurchaseOrderCommand
        {
            SupplierId = supplier.Id,
            OrderNumber = "po-001",
            OrderDate = DateTime.UtcNow
        };

        await Assert.ThrowsAsync<DuplicateOrderNumberException>(() => handler.HandleAsync(command, CancellationToken.None));
    }
}
