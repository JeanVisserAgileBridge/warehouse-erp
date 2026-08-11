using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Tests.Warehouses.StorageLocations.Fakes;
using WarehouseERP.Application.Warehouses.StorageLocations.Commands.UpdateStorageLocation;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.StorageLocations.Commands.UpdateStorageLocation;

public class UpdateStorageLocationCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_UpdatesStorageLocation_WhenValid()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseId = Guid.NewGuid();
        var storageLocation = StorageLocation.Create(warehouseId, "A-01");
        storageLocationRepository.Seed(storageLocation);

        var handler = new UpdateStorageLocationCommandHandler(storageLocationRepository);

        var command = new UpdateStorageLocationCommand
        {
            Id = storageLocation.Id,
            Code = "A-02",
            Description = "Aisle A, Shelf 2"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("A-02", dto.Code);
        Assert.Equal("Aisle A, Shelf 2", dto.Description);
        Assert.Equal(warehouseId, dto.WarehouseId);
        Assert.Equal("A-02", storageLocation.Code);
    }

    [Fact]
    public async Task HandleAsync_PreservesWarehouseId_WhenUpdating()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseId = Guid.NewGuid();
        var storageLocation = StorageLocation.Create(warehouseId, "A-01");
        storageLocationRepository.Seed(storageLocation);

        var handler = new UpdateStorageLocationCommandHandler(storageLocationRepository);

        var command = new UpdateStorageLocationCommand
        {
            Id = storageLocation.Id,
            Code = "A-02"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(warehouseId, dto.WarehouseId);
        Assert.Equal(warehouseId, storageLocation.WarehouseId);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenStorageLocationDoesNotExist()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var handler = new UpdateStorageLocationCommandHandler(storageLocationRepository);

        var command = new UpdateStorageLocationCommand
        {
            Id = Guid.NewGuid(),
            Code = "A-01"
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateCodeException_WhenCodeBelongsToAnotherStorageLocationInSameWarehouse()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseId = Guid.NewGuid();
        var storageLocationToUpdate = StorageLocation.Create(warehouseId, "A-01");
        var otherStorageLocation = StorageLocation.Create(warehouseId, "A-02");
        storageLocationRepository.Seed(storageLocationToUpdate);
        storageLocationRepository.Seed(otherStorageLocation);

        var handler = new UpdateStorageLocationCommandHandler(storageLocationRepository);

        var command = new UpdateStorageLocationCommand
        {
            Id = storageLocationToUpdate.Id,
            Code = "a-02"
        };

        await Assert.ThrowsAsync<DuplicateCodeException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_AllowsUpdate_WhenCodeIsUnchanged()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseId = Guid.NewGuid();
        var storageLocation = StorageLocation.Create(warehouseId, "A-01");
        storageLocationRepository.Seed(storageLocation);

        var handler = new UpdateStorageLocationCommandHandler(storageLocationRepository);

        var command = new UpdateStorageLocationCommand
        {
            Id = storageLocation.Id,
            Code = "A-01",
            Description = "Updated description"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("A-01", dto.Code);
        Assert.Equal("Updated description", dto.Description);
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToStorageLocationRepository()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A-01");
        storageLocationRepository.Seed(storageLocation);

        var handler = new UpdateStorageLocationCommandHandler(storageLocationRepository);

        var command = new UpdateStorageLocationCommand
        {
            Id = storageLocation.Id,
            Code = "A-01"
        };

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(command, cts.Token);

        Assert.Equal(cts.Token, storageLocationRepository.LastCancellationToken);
    }
}
