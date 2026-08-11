using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Tests.Warehouses.StorageLocations.Fakes;
using WarehouseERP.Application.Warehouses.StorageLocations.Commands.DeactivateStorageLocation;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.StorageLocations.Commands.DeactivateStorageLocation;

public class DeactivateStorageLocationCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_DeactivatesStorageLocation_WhenStorageLocationExists()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A-01");
        storageLocationRepository.Seed(storageLocation);

        var handler = new DeactivateStorageLocationCommandHandler(storageLocationRepository);

        var dto = await handler.HandleAsync(new DeactivateStorageLocationCommand { Id = storageLocation.Id }, CancellationToken.None);

        Assert.False(dto.IsActive);
        Assert.False(storageLocation.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenStorageLocationDoesNotExist()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var handler = new DeactivateStorageLocationCommandHandler(storageLocationRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new DeactivateStorageLocationCommand { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToStorageLocationRepository()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A-01");
        storageLocationRepository.Seed(storageLocation);

        var handler = new DeactivateStorageLocationCommandHandler(storageLocationRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new DeactivateStorageLocationCommand { Id = storageLocation.Id }, cts.Token);

        Assert.Equal(cts.Token, storageLocationRepository.LastCancellationToken);
    }
}
