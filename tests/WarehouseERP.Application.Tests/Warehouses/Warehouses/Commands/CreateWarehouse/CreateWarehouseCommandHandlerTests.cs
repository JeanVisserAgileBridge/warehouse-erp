using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Tests.Warehouses.Warehouses.Fakes;
using WarehouseERP.Application.Warehouses.Warehouses.Commands.CreateWarehouse;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.Warehouses.Commands.CreateWarehouse;

public class CreateWarehouseCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_AddsWarehouseToRepository_WhenCodeIsUnique()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var handler = new CreateWarehouseCommandHandler(warehouseRepository);

        var command = new CreateWarehouseCommand
        {
            Code = "WH-01",
            Name = "Main Warehouse"
        };

        await handler.HandleAsync(command, CancellationToken.None);

        var warehouses = await warehouseRepository.GetAllAsync(CancellationToken.None);
        Assert.Single(warehouses);
    }

    [Fact]
    public async Task HandleAsync_ReturnsMatchingWarehouseDto_WhenValid()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var handler = new CreateWarehouseCommandHandler(warehouseRepository);

        var command = new CreateWarehouseCommand
        {
            Code = "WH-01",
            Name = "Main Warehouse",
            Address = "1 Industrial Way"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("WH-01", dto.Code);
        Assert.Equal("Main Warehouse", dto.Name);
        Assert.Equal("1 Industrial Way", dto.Address);
        Assert.True(dto.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateCodeException_WhenCodeAlreadyExistsWithDifferentCase()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        warehouseRepository.Seed(Warehouse.Create("wh-01", "Main Warehouse"));

        var handler = new CreateWarehouseCommandHandler(warehouseRepository);

        var command = new CreateWarehouseCommand
        {
            Code = "WH-01",
            Name = "Secondary Warehouse"
        };

        await Assert.ThrowsAsync<DuplicateCodeException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToWarehouseRepository()
    {
        var warehouseRepository = new FakeWarehouseRepository();
        var handler = new CreateWarehouseCommandHandler(warehouseRepository);

        var command = new CreateWarehouseCommand
        {
            Code = "WH-01",
            Name = "Main Warehouse"
        };

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(command, cts.Token);

        Assert.Equal(cts.Token, warehouseRepository.LastCancellationToken);
    }
}
