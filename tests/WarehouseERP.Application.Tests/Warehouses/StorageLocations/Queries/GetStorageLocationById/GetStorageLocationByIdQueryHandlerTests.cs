using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Tests.Warehouses.StorageLocations.Fakes;
using WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocationById;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.StorageLocations.Queries.GetStorageLocationById;

public class GetStorageLocationByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsMatchingStorageLocationDto_WhenStorageLocationExists()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseId = Guid.NewGuid();
        var storageLocation = StorageLocation.Create(warehouseId, "A-01", "Aisle A, Shelf 1");
        storageLocationRepository.Seed(storageLocation);

        var handler = new GetStorageLocationByIdQueryHandler(storageLocationRepository);

        var dto = await handler.HandleAsync(new GetStorageLocationByIdQuery { Id = storageLocation.Id }, CancellationToken.None);

        Assert.Equal(storageLocation.Id, dto.Id);
        Assert.Equal(warehouseId, dto.WarehouseId);
        Assert.Equal(storageLocation.Code, dto.Code);
        Assert.Equal(storageLocation.Description, dto.Description);
        Assert.Equal(storageLocation.IsActive, dto.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenStorageLocationDoesNotExist()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var handler = new GetStorageLocationByIdQueryHandler(storageLocationRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new GetStorageLocationByIdQuery { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToStorageLocationRepository()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A-01");
        storageLocationRepository.Seed(storageLocation);

        var handler = new GetStorageLocationByIdQueryHandler(storageLocationRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new GetStorageLocationByIdQuery { Id = storageLocation.Id }, cts.Token);

        Assert.Equal(cts.Token, storageLocationRepository.LastCancellationToken);
    }
}
