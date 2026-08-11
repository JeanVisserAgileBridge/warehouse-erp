using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.ReceivePurchaseOrderLine;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Inventory.InventoryItems.Fakes;
using WarehouseERP.Application.Tests.Inventory.StockMovements.Fakes;
using WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Fakes;
using WarehouseERP.Application.Tests.Warehouses.StorageLocations.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Inventory;
using WarehouseERP.Domain.Procurement;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Commands.ReceivePurchaseOrderLine;

public class ReceivePurchaseOrderLineCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReceivesIntoExistingInventoryItem_WhenOneAlreadyExists()
    {
        var (handler, purchaseOrder, productId, storageLocation, inventoryItemRepository, _, _) = CreateSubmittedOrderWithHandler(quantityOrdered: 10);

        var existingInventoryItem = InventoryItem.Create(productId, storageLocation.Id, 5);
        inventoryItemRepository.Seed(existingInventoryItem);

        var command = new ReceivePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id
        };

        await handler.HandleAsync(command, CancellationToken.None);

        var inventoryItem = await inventoryItemRepository.GetByProductIdAndStorageLocationIdAsync(productId, storageLocation.Id, CancellationToken.None);
        Assert.Equal(9, inventoryItem!.QuantityOnHand);
        Assert.Equal(0, inventoryItemRepository.AddCallCount);
        Assert.Equal(1, inventoryItemRepository.UpdateCallCount);
    }

    [Fact]
    public async Task HandleAsync_CreatesNewInventoryItem_WhenNoneExists()
    {
        var (handler, purchaseOrder, productId, storageLocation, inventoryItemRepository, _, _) = CreateSubmittedOrderWithHandler(quantityOrdered: 10);

        var command = new ReceivePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = productId,
            Quantity = 6,
            StorageLocationId = storageLocation.Id
        };

        await handler.HandleAsync(command, CancellationToken.None);

        var inventoryItem = await inventoryItemRepository.GetByProductIdAndStorageLocationIdAsync(productId, storageLocation.Id, CancellationToken.None);
        Assert.NotNull(inventoryItem);
        Assert.Equal(6, inventoryItem!.QuantityOnHand);
        Assert.Equal(1, inventoryItemRepository.AddCallCount);
    }

    [Fact]
    public async Task HandleAsync_SetsPurchaseOrderStatusToPartiallyReceived_OnPartialReceipt()
    {
        var (handler, purchaseOrder, productId, storageLocation, _, _, _) = CreateSubmittedOrderWithHandler(quantityOrdered: 10);

        var command = new ReceivePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(PurchaseOrderStatus.PartiallyReceived, dto.Status);
    }

    [Fact]
    public async Task HandleAsync_SetsPurchaseOrderStatusToReceived_OnFullReceipt()
    {
        var (handler, purchaseOrder, productId, storageLocation, _, _, _) = CreateSubmittedOrderWithHandler(quantityOrdered: 10);

        var command = new ReceivePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = productId,
            Quantity = 10,
            StorageLocationId = storageLocation.Id
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(PurchaseOrderStatus.Received, dto.Status);
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenReceivingMoreThanOrdered()
    {
        var (handler, purchaseOrder, productId, storageLocation, _, _, _) = CreateSubmittedOrderWithHandler(quantityOrdered: 10);

        var command = new ReceivePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = productId,
            Quantity = 11,
            StorageLocationId = storageLocation.Id
        };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenPurchaseOrderIsStillDraft()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var storageLocationRepository = new FakeStorageLocationRepository();
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var productId = Guid.NewGuid();
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrder.AddLine(productId, 10, 5.00m);
        purchaseOrderRepository.Seed(purchaseOrder);

        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1");
        storageLocationRepository.Seed(storageLocation);

        var handler = new ReceivePurchaseOrderLineCommandHandler(
            purchaseOrderRepository, storageLocationRepository, inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new ReceivePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id
        };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenPurchaseOrderDoesNotExist()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var storageLocationRepository = new FakeStorageLocationRepository();
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new ReceivePurchaseOrderLineCommandHandler(
            purchaseOrderRepository, storageLocationRepository, inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new ReceivePurchaseOrderLineCommand
        {
            PurchaseOrderId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 4,
            StorageLocationId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenStorageLocationDoesNotExist()
    {
        var (handler, purchaseOrder, productId, _, _, _, _) = CreateSubmittedOrderWithHandler(quantityOrdered: 10);

        var command = new ReceivePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsInactiveStorageLocationException_WhenStorageLocationIsNotActive()
    {
        var (handler, purchaseOrder, productId, storageLocation, _, _, _) = CreateSubmittedOrderWithHandler(quantityOrdered: 10);
        storageLocation.Deactivate();

        var command = new ReceivePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id
        };

        await Assert.ThrowsAsync<InactiveStorageLocationException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_CreatesReceiptStockMovement()
    {
        var (handler, purchaseOrder, productId, storageLocation, inventoryItemRepository, stockMovementRepository, _) =
            CreateSubmittedOrderWithHandler(quantityOrdered: 10);

        var command = new ReceivePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id,
            Reference = "PO-001"
        };

        await handler.HandleAsync(command, CancellationToken.None);

        var inventoryItem = await inventoryItemRepository.GetByProductIdAndStorageLocationIdAsync(productId, storageLocation.Id, CancellationToken.None);
        var movements = await stockMovementRepository.GetByInventoryItemIdAsync(inventoryItem!.Id, CancellationToken.None);
        var movement = Assert.Single(movements);
        Assert.Equal(StockMovementType.Receipt, movement.MovementType);
        Assert.Equal(4, movement.Quantity);
        Assert.Equal("PO-001", movement.Reference);
    }

    [Fact]
    public async Task HandleAsync_CommitsAllChangesInOneUnitOfWorkCall()
    {
        var (handler, purchaseOrder, productId, storageLocation, inventoryItemRepository, stockMovementRepository, unitOfWork) =
            CreateSubmittedOrderWithHandler(quantityOrdered: 10);

        var command = new ReceivePurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id
        };

        await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(1, inventoryItemRepository.AddCallCount);
        Assert.Equal(1, stockMovementRepository.AddCallCount);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    private static (
        ReceivePurchaseOrderLineCommandHandler Handler,
        PurchaseOrder PurchaseOrder,
        Guid ProductId,
        StorageLocation StorageLocation,
        FakeInventoryItemRepository InventoryItemRepository,
        FakeStockMovementRepository StockMovementRepository,
        FakeUnitOfWork UnitOfWork) CreateSubmittedOrderWithHandler(int quantityOrdered)
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var storageLocationRepository = new FakeStorageLocationRepository();
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var productId = Guid.NewGuid();
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrder.AddLine(productId, quantityOrdered, 5.00m);
        purchaseOrder.Submit();
        purchaseOrderRepository.Seed(purchaseOrder);

        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1");
        storageLocationRepository.Seed(storageLocation);

        var handler = new ReceivePurchaseOrderLineCommandHandler(
            purchaseOrderRepository, storageLocationRepository, inventoryItemRepository, stockMovementRepository, unitOfWork);

        return (handler, purchaseOrder, productId, storageLocation, inventoryItemRepository, stockMovementRepository, unitOfWork);
    }
}
