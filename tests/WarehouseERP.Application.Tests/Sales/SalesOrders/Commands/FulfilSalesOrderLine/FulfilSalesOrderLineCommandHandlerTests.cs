using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.SalesOrders.Commands.FulfilSalesOrderLine;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Inventory.InventoryItems.Fakes;
using WarehouseERP.Application.Tests.Inventory.StockMovements.Fakes;
using WarehouseERP.Application.Tests.Sales.SalesOrders.Fakes;
using WarehouseERP.Application.Tests.Warehouses.StorageLocations.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Inventory;
using WarehouseERP.Domain.Sales;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Sales.SalesOrders.Commands.FulfilSalesOrderLine;

public class FulfilSalesOrderLineCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_IssuesStock_WhenInventoryItemExistsWithSufficientQuantity()
    {
        var (handler, salesOrder, productId, storageLocation, inventoryItemRepository, _, _) =
            CreateConfirmedOrderWithHandler(quantityOrdered: 10);

        var inventoryItem = InventoryItem.Create(productId, storageLocation.Id, 20);
        inventoryItemRepository.Seed(inventoryItem);

        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id
        };

        await handler.HandleAsync(command, CancellationToken.None);

        var updatedInventoryItem = await inventoryItemRepository.GetByProductIdAndStorageLocationIdAsync(
            productId, storageLocation.Id, CancellationToken.None);
        Assert.Equal(16, updatedInventoryItem!.QuantityOnHand);
        Assert.Equal(1, inventoryItemRepository.UpdateCallCount);
    }

    [Fact]
    public async Task HandleAsync_SetsSalesOrderStatusToPartiallyFulfilled_OnPartialFulfilment()
    {
        var (handler, salesOrder, productId, storageLocation, inventoryItemRepository, _, _) =
            CreateConfirmedOrderWithHandler(quantityOrdered: 10);

        inventoryItemRepository.Seed(InventoryItem.Create(productId, storageLocation.Id, 20));

        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(SalesOrderStatus.PartiallyFulfilled, dto.Status);
    }

    [Fact]
    public async Task HandleAsync_SetsSalesOrderStatusToFulfilled_OnFullFulfilment()
    {
        var (handler, salesOrder, productId, storageLocation, inventoryItemRepository, _, _) =
            CreateConfirmedOrderWithHandler(quantityOrdered: 10);

        inventoryItemRepository.Seed(InventoryItem.Create(productId, storageLocation.Id, 20));

        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            Quantity = 10,
            StorageLocationId = storageLocation.Id
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(SalesOrderStatus.Fulfilled, dto.Status);
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenFulfillingMoreThanOrdered()
    {
        var (handler, salesOrder, productId, storageLocation, inventoryItemRepository, _, _) =
            CreateConfirmedOrderWithHandler(quantityOrdered: 10);

        inventoryItemRepository.Seed(InventoryItem.Create(productId, storageLocation.Id, 20));

        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            Quantity = 11,
            StorageLocationId = storageLocation.Id
        };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenSalesOrderIsStillDraft()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var storageLocationRepository = new FakeStorageLocationRepository();
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var productId = Guid.NewGuid();
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrder.AddLine(productId, 10, 5.00m);
        salesOrderRepository.Seed(salesOrder);

        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1");
        storageLocationRepository.Seed(storageLocation);

        var handler = new FulfilSalesOrderLineCommandHandler(
            salesOrderRepository, storageLocationRepository, inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id
        };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenSalesOrderDoesNotExist()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var storageLocationRepository = new FakeStorageLocationRepository();
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new FulfilSalesOrderLineCommandHandler(
            salesOrderRepository, storageLocationRepository, inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 4,
            StorageLocationId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenStorageLocationDoesNotExist()
    {
        var (handler, salesOrder, productId, _, _, _, _) = CreateConfirmedOrderWithHandler(quantityOrdered: 10);

        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsInactiveStorageLocationException_WhenStorageLocationIsNotActive()
    {
        var (handler, salesOrder, productId, storageLocation, inventoryItemRepository, _, _) =
            CreateConfirmedOrderWithHandler(quantityOrdered: 10);
        inventoryItemRepository.Seed(InventoryItem.Create(productId, storageLocation.Id, 20));
        storageLocation.Deactivate();

        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id
        };

        await Assert.ThrowsAsync<InactiveStorageLocationException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenInventoryItemDoesNotExist()
    {
        var (handler, salesOrder, productId, storageLocation, _, _, _) = CreateConfirmedOrderWithHandler(quantityOrdered: 10);

        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenStockIsInsufficient()
    {
        var (handler, salesOrder, productId, storageLocation, inventoryItemRepository, _, _) =
            CreateConfirmedOrderWithHandler(quantityOrdered: 10);

        inventoryItemRepository.Seed(InventoryItem.Create(productId, storageLocation.Id, 2));

        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id
        };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_CreatesIssueStockMovement()
    {
        var (handler, salesOrder, productId, storageLocation, inventoryItemRepository, stockMovementRepository, _) =
            CreateConfirmedOrderWithHandler(quantityOrdered: 10);

        inventoryItemRepository.Seed(InventoryItem.Create(productId, storageLocation.Id, 20));

        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id,
            Reference = "SO-001"
        };

        await handler.HandleAsync(command, CancellationToken.None);

        var inventoryItem = await inventoryItemRepository.GetByProductIdAndStorageLocationIdAsync(
            productId, storageLocation.Id, CancellationToken.None);
        var movements = await stockMovementRepository.GetByInventoryItemIdAsync(inventoryItem!.Id, CancellationToken.None);
        var movement = Assert.Single(movements);
        Assert.Equal(StockMovementType.Issue, movement.MovementType);
        Assert.Equal(4, movement.Quantity);
        Assert.Equal("SO-001", movement.Reference);
    }

    [Fact]
    public async Task HandleAsync_CommitsAllChangesInOneUnitOfWorkCall()
    {
        var (handler, salesOrder, productId, storageLocation, inventoryItemRepository, stockMovementRepository, unitOfWork) =
            CreateConfirmedOrderWithHandler(quantityOrdered: 10);

        inventoryItemRepository.Seed(InventoryItem.Create(productId, storageLocation.Id, 20));

        var command = new FulfilSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            Quantity = 4,
            StorageLocationId = storageLocation.Id
        };

        await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(1, inventoryItemRepository.UpdateCallCount);
        Assert.Equal(1, stockMovementRepository.AddCallCount);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    private static (
        FulfilSalesOrderLineCommandHandler Handler,
        SalesOrder SalesOrder,
        Guid ProductId,
        StorageLocation StorageLocation,
        FakeInventoryItemRepository InventoryItemRepository,
        FakeStockMovementRepository StockMovementRepository,
        FakeUnitOfWork UnitOfWork) CreateConfirmedOrderWithHandler(int quantityOrdered)
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var storageLocationRepository = new FakeStorageLocationRepository();
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var productId = Guid.NewGuid();
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrder.AddLine(productId, quantityOrdered, 5.00m);
        salesOrder.Confirm();
        salesOrderRepository.Seed(salesOrder);

        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1");
        storageLocationRepository.Seed(storageLocation);

        var handler = new FulfilSalesOrderLineCommandHandler(
            salesOrderRepository, storageLocationRepository, inventoryItemRepository, stockMovementRepository, unitOfWork);

        return (handler, salesOrder, productId, storageLocation, inventoryItemRepository, stockMovementRepository, unitOfWork);
    }
}
