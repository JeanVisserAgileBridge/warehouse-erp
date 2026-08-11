using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Tests.Warehouses.Warehouses.Fakes;
using WarehouseERP.Application.Warehouses.Warehouses.Queries.GetWarehouseById;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.Warehouses.Queries.GetWarehouseById;

public class GetWarehouseByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsMatchingWarehouseDto_WhenWarehouseExists()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse", "1 Industrial Way");
        warehouseRepository.Seed(warehouse);

        var handler = new GetWarehouseByIdQueryHandler(warehouseRepository);

        var dto = await handler.HandleAsync(new GetWarehouseByIdQuery { Id = warehouse.Id }, CancellationToken.None);

        Assert.Equal(warehouse.Id, dto.Id);
        Assert.Equal(warehouse.Code, dto.Code);
        Assert.Equal(warehouse.Name, dto.Name);
        Assert.Equal(warehouse.Address, dto.Address);
        Assert.Equal(warehouse.IsActive, dto.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenWarehouseDoesNotExist()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var handler = new GetWarehouseByIdQueryHandler(warehouseRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new GetWarehouseByIdQuery { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToWarehouseRepository()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouseRepository.Seed(warehouse);

        var handler = new GetWarehouseByIdQueryHandler(warehouseRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new GetWarehouseByIdQuery { Id = warehouse.Id }, cts.Token);

        Assert.Equal(cts.Token, warehouseRepository.LastCancellationToken);
    }
}
