using WarehouseERP.Application.Tests.Warehouses.StorageLocations.Fakes;
using WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocationsByWarehouseId;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.StorageLocations.Queries.GetStorageLocationsByWarehouseId;

public class GetStorageLocationsByWarehouseIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsOnlyStorageLocationsForGivenWarehouse()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var warehouseId = Guid.NewGuid();
        var otherWarehouseId = Guid.NewGuid();
        storageLocationRepository.Seed(StorageLocation.Create(warehouseId, "A-01"));
        storageLocationRepository.Seed(StorageLocation.Create(warehouseId, "A-02"));
        storageLocationRepository.Seed(StorageLocation.Create(otherWarehouseId, "B-01"));

        var handler = new GetStorageLocationsByWarehouseIdQueryHandler(storageLocationRepository);

        var dtos = await handler.HandleAsync(
            new GetStorageLocationsByWarehouseIdQuery { WarehouseId = warehouseId }, CancellationToken.None);

        Assert.Equal(2, dtos.Count);
        Assert.All(dtos, d => Assert.Equal(warehouseId, d.WarehouseId));
    }

    [Fact]
    public async Task HandleAsync_ReturnsEmptyList_WhenWarehouseHasNoStorageLocations()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var handler = new GetStorageLocationsByWarehouseIdQueryHandler(storageLocationRepository);

        var dtos = await handler.HandleAsync(
            new GetStorageLocationsByWarehouseIdQuery { WarehouseId = Guid.NewGuid() }, CancellationToken.None);

        Assert.Empty(dtos);
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToStorageLocationRepository()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var handler = new GetStorageLocationsByWarehouseIdQueryHandler(storageLocationRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new GetStorageLocationsByWarehouseIdQuery { WarehouseId = Guid.NewGuid() }, cts.Token);

        Assert.Equal(cts.Token, storageLocationRepository.LastCancellationToken);
    }
}
