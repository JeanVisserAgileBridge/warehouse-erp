using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.SalesOrders.Commands.ConfirmSalesOrder;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Sales.SalesOrders.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.SalesOrders.Commands.ConfirmSalesOrder;

public class ConfirmSalesOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ConfirmsOrder_WhenDraftWithLines()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrder.AddLine(Guid.NewGuid(), 10, 5.00m);
        salesOrderRepository.Seed(salesOrder);

        var handler = new ConfirmSalesOrderCommandHandler(salesOrderRepository, unitOfWork);

        var dto = await handler.HandleAsync(
            new ConfirmSalesOrderCommand { SalesOrderId = salesOrder.Id }, CancellationToken.None);

        Assert.Equal(SalesOrderStatus.Confirmed, dto.Status);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenSalesOrderDoesNotExist()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new ConfirmSalesOrderCommandHandler(salesOrderRepository, unitOfWork);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(
            new ConfirmSalesOrderCommand { SalesOrderId = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenOrderHasNoLines()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrderRepository.Seed(salesOrder);

        var handler = new ConfirmSalesOrderCommandHandler(salesOrderRepository, unitOfWork);

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(
            new ConfirmSalesOrderCommand { SalesOrderId = salesOrder.Id }, CancellationToken.None));
    }
}
