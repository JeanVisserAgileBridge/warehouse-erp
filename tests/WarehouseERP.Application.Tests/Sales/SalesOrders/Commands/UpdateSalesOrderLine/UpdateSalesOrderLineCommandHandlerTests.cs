using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.SalesOrders.Commands.UpdateSalesOrderLine;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Sales.SalesOrders.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.SalesOrders.Commands.UpdateSalesOrderLine;

public class UpdateSalesOrderLineCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_UpdatesQuantityAndPrice_WhenOrderIsDraft()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var productId = Guid.NewGuid();
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrder.AddLine(productId, 10, 5.00m);
        salesOrderRepository.Seed(salesOrder);

        var handler = new UpdateSalesOrderLineCommandHandler(salesOrderRepository, unitOfWork);

        var command = new UpdateSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            QuantityOrdered = 20,
            UnitPrice = 6.50m
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        var line = Assert.Single(dto.Lines);
        Assert.Equal(20, line.QuantityOrdered);
        Assert.Equal(6.50m, line.UnitPrice);
        Assert.Equal(0, line.QuantityFulfilled);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenSalesOrderDoesNotExist()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new UpdateSalesOrderLineCommandHandler(salesOrderRepository, unitOfWork);

        var command = new UpdateSalesOrderLineCommand
        {
            SalesOrderId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            QuantityOrdered = 5,
            UnitPrice = 1.00m
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenOrderIsNotDraft()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var productId = Guid.NewGuid();
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrder.AddLine(productId, 10, 5.00m);
        salesOrder.Confirm();
        salesOrderRepository.Seed(salesOrder);

        var handler = new UpdateSalesOrderLineCommandHandler(salesOrderRepository, unitOfWork);

        var command = new UpdateSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = productId,
            QuantityOrdered = 20,
            UnitPrice = 6.50m
        };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenProductIsNotOnOrder()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrderRepository.Seed(salesOrder);

        var handler = new UpdateSalesOrderLineCommandHandler(salesOrderRepository, unitOfWork);

        var command = new UpdateSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = Guid.NewGuid(),
            QuantityOrdered = 20,
            UnitPrice = 6.50m
        };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));
    }
}
