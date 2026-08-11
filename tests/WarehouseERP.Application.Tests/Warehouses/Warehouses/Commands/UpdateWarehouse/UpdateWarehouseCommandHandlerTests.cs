using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Tests.Warehouses.Warehouses.Fakes;
using WarehouseERP.Application.Warehouses.Warehouses.Commands.UpdateWarehouse;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.Warehouses.Commands.UpdateWarehouse;

public class UpdateWarehouseCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_UpdatesWarehouse_WhenValid()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouseRepository.Seed(warehouse);

        var handler = new UpdateWarehouseCommandHandler(warehouseRepository);

        var command = new UpdateWarehouseCommand
        {
            Id = warehouse.Id,
            Code = "WH-01A",
            Name = "Main Warehouse Updated",
            Address = "2 Industrial Way"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("WH-01A", dto.Code);
        Assert.Equal("Main Warehouse Updated", dto.Name);
        Assert.Equal("2 Industrial Way", dto.Address);
        Assert.Equal("WH-01A", warehouse.Code);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenWarehouseDoesNotExist()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var handler = new UpdateWarehouseCommandHandler(warehouseRepository);

        var command = new UpdateWarehouseCommand
        {
            Id = Guid.NewGuid(),
            Code = "WH-01",
            Name = "Main Warehouse"
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateCodeException_WhenCodeBelongsToAnotherWarehouse()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouseToUpdate = Warehouse.Create("WH-01", "Main Warehouse");
        var otherWarehouse = Warehouse.Create("WH-02", "Secondary Warehouse");
        warehouseRepository.Seed(warehouseToUpdate);
        warehouseRepository.Seed(otherWarehouse);

        var handler = new UpdateWarehouseCommandHandler(warehouseRepository);

        var command = new UpdateWarehouseCommand
        {
            Id = warehouseToUpdate.Id,
            Code = "wh-02",
            Name = "Main Warehouse"
        };

        await Assert.ThrowsAsync<DuplicateCodeException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_AllowsUpdate_WhenCodeIsUnchanged()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouseRepository.Seed(warehouse);

        var handler = new UpdateWarehouseCommandHandler(warehouseRepository);

        var command = new UpdateWarehouseCommand
        {
            Id = warehouse.Id,
            Code = "WH-01",
            Name = "Main Warehouse Updated"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("WH-01", dto.Code);
        Assert.Equal("Main Warehouse Updated", dto.Name);
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToWarehouseRepository()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouseRepository.Seed(warehouse);

        var handler = new UpdateWarehouseCommandHandler(warehouseRepository);

        var command = new UpdateWarehouseCommand
        {
            Id = warehouse.Id,
            Code = "WH-01",
            Name = "Main Warehouse"
        };

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(command, cts.Token);

        Assert.Equal(cts.Token, warehouseRepository.LastCancellationToken);
    }
}
