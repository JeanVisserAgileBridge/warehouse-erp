using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.CreateInventoryItem;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Inventory.InventoryItems.Fakes;
using WarehouseERP.Application.Tests.ProductCatalog.Products.Fakes;
using WarehouseERP.Application.Tests.Warehouses.StorageLocations.Fakes;
using WarehouseERP.Domain.ProductCatalog;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Inventory.InventoryItems.Commands.CreateInventoryItem;

public class CreateInventoryItemCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_AddsInventoryItemToRepository_WhenValid()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var productRepository = new FakeProductRepository();
        var storageLocationRepository = new FakeStorageLocationRepository();
        var unitOfWork = new FakeUnitOfWork();

        var product = Product.Create("SKU-01", "Widget", Guid.NewGuid(), 9.99m);
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A-01");
        productRepository.Seed(product);
        storageLocationRepository.Seed(storageLocation);

        var handler = new CreateInventoryItemCommandHandler(
            inventoryItemRepository, productRepository, storageLocationRepository, unitOfWork);

        var command = new CreateInventoryItemCommand
        {
            ProductId = product.Id,
            StorageLocationId = storageLocation.Id,
            QuantityOnHand = 10,
            ReorderLevel = 2
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(product.Id, dto.ProductId);
        Assert.Equal(storageLocation.Id, dto.StorageLocationId);
        Assert.Equal(10, dto.QuantityOnHand);
        Assert.Equal(2, dto.ReorderLevel);
        Assert.Equal(1, inventoryItemRepository.AddCallCount);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var productRepository = new FakeProductRepository();
        var storageLocationRepository = new FakeStorageLocationRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new CreateInventoryItemCommandHandler(
            inventoryItemRepository, productRepository, storageLocationRepository, unitOfWork);

        var command = new CreateInventoryItemCommand
        {
            ProductId = Guid.NewGuid(),
            StorageLocationId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsInactiveProductException_WhenProductIsNotActive()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var productRepository = new FakeProductRepository();
        var storageLocationRepository = new FakeStorageLocationRepository();
        var unitOfWork = new FakeUnitOfWork();

        var product = Product.Create("SKU-01", "Widget", Guid.NewGuid(), 9.99m);
        product.Deactivate();
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A-01");
        productRepository.Seed(product);
        storageLocationRepository.Seed(storageLocation);

        var handler = new CreateInventoryItemCommandHandler(
            inventoryItemRepository, productRepository, storageLocationRepository, unitOfWork);

        var command = new CreateInventoryItemCommand
        {
            ProductId = product.Id,
            StorageLocationId = storageLocation.Id
        };

        await Assert.ThrowsAsync<InactiveProductException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenStorageLocationDoesNotExist()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var productRepository = new FakeProductRepository();
        var storageLocationRepository = new FakeStorageLocationRepository();
        var unitOfWork = new FakeUnitOfWork();

        var product = Product.Create("SKU-01", "Widget", Guid.NewGuid(), 9.99m);
        productRepository.Seed(product);

        var handler = new CreateInventoryItemCommandHandler(
            inventoryItemRepository, productRepository, storageLocationRepository, unitOfWork);

        var command = new CreateInventoryItemCommand
        {
            ProductId = product.Id,
            StorageLocationId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsInactiveStorageLocationException_WhenStorageLocationIsNotActive()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var productRepository = new FakeProductRepository();
        var storageLocationRepository = new FakeStorageLocationRepository();
        var unitOfWork = new FakeUnitOfWork();

        var product = Product.Create("SKU-01", "Widget", Guid.NewGuid(), 9.99m);
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A-01");
        storageLocation.Deactivate();
        productRepository.Seed(product);
        storageLocationRepository.Seed(storageLocation);

        var handler = new CreateInventoryItemCommandHandler(
            inventoryItemRepository, productRepository, storageLocationRepository, unitOfWork);

        var command = new CreateInventoryItemCommand
        {
            ProductId = product.Id,
            StorageLocationId = storageLocation.Id
        };

        await Assert.ThrowsAsync<InactiveStorageLocationException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateInventoryItemException_WhenProductAndStorageLocationCombinationAlreadyExists()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var productRepository = new FakeProductRepository();
        var storageLocationRepository = new FakeStorageLocationRepository();
        var unitOfWork = new FakeUnitOfWork();

        var product = Product.Create("SKU-01", "Widget", Guid.NewGuid(), 9.99m);
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A-01");
        productRepository.Seed(product);
        storageLocationRepository.Seed(storageLocation);
        inventoryItemRepository.Seed(Domain.Inventory.InventoryItem.Create(product.Id, storageLocation.Id));

        var handler = new CreateInventoryItemCommandHandler(
            inventoryItemRepository, productRepository, storageLocationRepository, unitOfWork);

        var command = new CreateInventoryItemCommand
        {
            ProductId = product.Id,
            StorageLocationId = storageLocation.Id
        };

        await Assert.ThrowsAsync<DuplicateInventoryItemException>(() => handler.HandleAsync(command, CancellationToken.None));
    }
}
