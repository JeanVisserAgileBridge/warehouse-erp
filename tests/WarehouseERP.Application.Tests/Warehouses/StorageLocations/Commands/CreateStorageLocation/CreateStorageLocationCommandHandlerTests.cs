using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Tests.Warehouses.StorageLocations.Fakes;
using WarehouseERP.Application.Tests.Warehouses.Warehouses.Fakes;
using WarehouseERP.Application.Warehouses.StorageLocations.Commands.CreateStorageLocation;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.StorageLocations.Commands.CreateStorageLocation;

public class CreateStorageLocationCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_AddsStorageLocationToRepository_WhenValid()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouseRepository.Seed(warehouse);

        var handler = new CreateStorageLocationCommandHandler(storageLocationRepository, warehouseRepository);

        var command = new CreateStorageLocationCommand
        {
            WarehouseId = warehouse.Id,
            Code = "A-01"
        };

        await handler.HandleAsync(command, CancellationToken.None);

        var storageLocations = await storageLocationRepository.GetAllAsync(CancellationToken.None);
        Assert.Single(storageLocations);
    }

    [Fact]
    public async Task HandleAsync_ReturnsMatchingStorageLocationDto_WhenValid()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouseRepository.Seed(warehouse);

        var handler = new CreateStorageLocationCommandHandler(storageLocationRepository, warehouseRepository);

        var command = new CreateStorageLocationCommand
        {
            WarehouseId = warehouse.Id,
            Code = "A-01",
            Description = "Aisle A, Shelf 1"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(warehouse.Id, dto.WarehouseId);
        Assert.Equal("A-01", dto.Code);
        Assert.Equal("Aisle A, Shelf 1", dto.Description);
        Assert.True(dto.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenWarehouseDoesNotExist()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseRepository = new FakeWarehouseRepository();

        var handler = new CreateStorageLocationCommandHandler(storageLocationRepository, warehouseRepository);

        var command = new CreateStorageLocationCommand
        {
            WarehouseId = Guid.NewGuid(),
            Code = "A-01"
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsInactiveWarehouseException_WhenWarehouseIsNotActive()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouse.Deactivate();
        warehouseRepository.Seed(warehouse);

        var handler = new CreateStorageLocationCommandHandler(storageLocationRepository, warehouseRepository);

        var command = new CreateStorageLocationCommand
        {
            WarehouseId = warehouse.Id,
            Code = "A-01"
        };

        await Assert.ThrowsAsync<InactiveWarehouseException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateCodeException_WhenCodeAlreadyExistsInSameWarehouseWithDifferentCase()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouseRepository.Seed(warehouse);
        storageLocationRepository.Seed(StorageLocation.Create(warehouse.Id, "a-01"));

        var handler = new CreateStorageLocationCommandHandler(storageLocationRepository, warehouseRepository);

        var command = new CreateStorageLocationCommand
        {
            WarehouseId = warehouse.Id,
            Code = "A-01"
        };

        await Assert.ThrowsAsync<DuplicateCodeException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_AllowsSameCode_InDifferentWarehouses()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseRepository = new FakeWarehouseRepository();
        var firstWarehouse = Warehouse.Create("WH-01", "Main Warehouse");
        var secondWarehouse = Warehouse.Create("WH-02", "Secondary Warehouse");
        warehouseRepository.Seed(firstWarehouse);
        warehouseRepository.Seed(secondWarehouse);
        storageLocationRepository.Seed(StorageLocation.Create(firstWarehouse.Id, "A-01"));

        var handler = new CreateStorageLocationCommandHandler(storageLocationRepository, warehouseRepository);

        var command = new CreateStorageLocationCommand
        {
            WarehouseId = secondWarehouse.Id,
            Code = "A-01"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(secondWarehouse.Id, dto.WarehouseId);
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToStorageLocationRepository()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouseRepository.Seed(warehouse);

        var handler = new CreateStorageLocationCommandHandler(storageLocationRepository, warehouseRepository);

        var command = new CreateStorageLocationCommand
        {
            WarehouseId = warehouse.Id,
            Code = "A-01"
        };

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(command, cts.Token);

        Assert.Equal(cts.Token, storageLocationRepository.LastCancellationToken);
    }
}
