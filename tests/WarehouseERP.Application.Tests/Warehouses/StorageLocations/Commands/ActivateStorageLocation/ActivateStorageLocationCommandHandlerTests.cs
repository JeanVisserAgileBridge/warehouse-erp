using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Tests.Warehouses.StorageLocations.Fakes;
using WarehouseERP.Application.Warehouses.StorageLocations.Commands.ActivateStorageLocation;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.StorageLocations.Commands.ActivateStorageLocation;

public class ActivateStorageLocationCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ActivatesStorageLocation_WhenStorageLocationExists()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A-01");
        storageLocation.Deactivate();
        storageLocationRepository.Seed(storageLocation);

        var handler = new ActivateStorageLocationCommandHandler(storageLocationRepository);

        var dto = await handler.HandleAsync(new ActivateStorageLocationCommand { Id = storageLocation.Id }, CancellationToken.None);

        Assert.True(dto.IsActive);
        Assert.True(storageLocation.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenStorageLocationDoesNotExist()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var handler = new ActivateStorageLocationCommandHandler(storageLocationRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new ActivateStorageLocationCommand { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToStorageLocationRepository()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A-01");
        storageLocationRepository.Seed(storageLocation);

        var handler = new ActivateStorageLocationCommandHandler(storageLocationRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new ActivateStorageLocationCommand { Id = storageLocation.Id }, cts.Token);

        Assert.Equal(cts.Token, storageLocationRepository.LastCancellationToken);
    }
}
