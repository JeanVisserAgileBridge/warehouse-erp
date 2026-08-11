using WarehouseERP.Application.Tests.Warehouses.StorageLocations.Fakes;
using WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocations;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.StorageLocations.Queries.GetStorageLocations;

public class GetStorageLocationsQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsAllStorageLocationsAsDtos()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var first = StorageLocation.Create(Guid.NewGuid(), "A-01");
        var second = StorageLocation.Create(Guid.NewGuid(), "B-01");
        storageLocationRepository.Seed(first);
        storageLocationRepository.Seed(second);

        var handler = new GetStorageLocationsQueryHandler(storageLocationRepository);

        var dtos = await handler.HandleAsync(new GetStorageLocationsQuery(), CancellationToken.None);

        Assert.Equal(2, dtos.Count);
        Assert.Contains(dtos, d => d.Code == "A-01");
        Assert.Contains(dtos, d => d.Code == "B-01");
    }

    [Fact]
    public async Task HandleAsync_ReturnsEmptyList_WhenNoStorageLocationsExist()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var handler = new GetStorageLocationsQueryHandler(storageLocationRepository);

        var dtos = await handler.HandleAsync(new GetStorageLocationsQuery(), CancellationToken.None);

        Assert.Empty(dtos);
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToStorageLocationRepository()
    {
        var storageLocationRepository = new FakeStorageLocationRepository();
        var handler = new GetStorageLocationsQueryHandler(storageLocationRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new GetStorageLocationsQuery(), cts.Token);

        Assert.Equal(cts.Token, storageLocationRepository.LastCancellationToken);
    }
}
