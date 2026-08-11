using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Tests.Warehouses.Warehouses.Fakes;
using WarehouseERP.Application.Warehouses.Warehouses.Commands.DeactivateWarehouse;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.Warehouses.Commands.DeactivateWarehouse;

public class DeactivateWarehouseCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_DeactivatesWarehouse_WhenWarehouseExists()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouseRepository.Seed(warehouse);

        var handler = new DeactivateWarehouseCommandHandler(warehouseRepository);

        var dto = await handler.HandleAsync(new DeactivateWarehouseCommand { Id = warehouse.Id }, CancellationToken.None);

        Assert.False(dto.IsActive);
        Assert.False(warehouse.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenWarehouseDoesNotExist()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var handler = new DeactivateWarehouseCommandHandler(warehouseRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new DeactivateWarehouseCommand { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToWarehouseRepository()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouseRepository.Seed(warehouse);

        var handler = new DeactivateWarehouseCommandHandler(warehouseRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new DeactivateWarehouseCommand { Id = warehouse.Id }, cts.Token);

        Assert.Equal(cts.Token, warehouseRepository.LastCancellationToken);
    }
}
