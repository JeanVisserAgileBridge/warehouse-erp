using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Tests.Warehouses.Warehouses.Fakes;
using WarehouseERP.Application.Warehouses.Warehouses.Commands.ActivateWarehouse;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.Warehouses.Commands.ActivateWarehouse;

public class ActivateWarehouseCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ActivatesWarehouse_WhenWarehouseExists()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouse.Deactivate();
        warehouseRepository.Seed(warehouse);

        var handler = new ActivateWarehouseCommandHandler(warehouseRepository);

        var dto = await handler.HandleAsync(new ActivateWarehouseCommand { Id = warehouse.Id }, CancellationToken.None);

        Assert.True(dto.IsActive);
        Assert.True(warehouse.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenWarehouseDoesNotExist()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var handler = new ActivateWarehouseCommandHandler(warehouseRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new ActivateWarehouseCommand { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToWarehouseRepository()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouseRepository.Seed(warehouse);

        var handler = new ActivateWarehouseCommandHandler(warehouseRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new ActivateWarehouseCommand { Id = warehouse.Id }, cts.Token);

        Assert.Equal(cts.Token, warehouseRepository.LastCancellationToken);
    }
}
