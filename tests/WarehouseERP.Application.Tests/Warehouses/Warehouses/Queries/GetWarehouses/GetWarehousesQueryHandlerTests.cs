using WarehouseERP.Application.Tests.Warehouses.Warehouses.Fakes;
using WarehouseERP.Application.Warehouses.Warehouses.Queries.GetWarehouses;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.Warehouses.Queries.GetWarehouses;

public class GetWarehousesQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsAllWarehousesAsDtos()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var first = Warehouse.Create("WH-01", "Main Warehouse");
        var second = Warehouse.Create("WH-02", "Secondary Warehouse");
        warehouseRepository.Seed(first);
        warehouseRepository.Seed(second);

        var handler = new GetWarehousesQueryHandler(warehouseRepository);

        var dtos = await handler.HandleAsync(new GetWarehousesQuery(), CancellationToken.None);

        Assert.Equal(2, dtos.Count);
        Assert.Contains(dtos, d => d.Code == "WH-01");
        Assert.Contains(dtos, d => d.Code == "WH-02");
    }

    [Fact]
    public async Task HandleAsync_ReturnsEmptyList_WhenNoWarehousesExist()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var handler = new GetWarehousesQueryHandler(warehouseRepository);

        var dtos = await handler.HandleAsync(new GetWarehousesQuery(), CancellationToken.None);

        Assert.Empty(dtos);
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToWarehouseRepository()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var handler = new GetWarehousesQueryHandler(warehouseRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new GetWarehousesQuery(), cts.Token);

        Assert.Equal(cts.Token, warehouseRepository.LastCancellationToken);
    }
}
