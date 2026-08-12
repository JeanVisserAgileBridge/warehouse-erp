using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.SalesOrders.Commands.CancelSalesOrder;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Sales.SalesOrders.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.SalesOrders.Commands.CancelSalesOrder;

public class CancelSalesOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_CancelsOrder_WhenNotYetFulfilled()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrder.AddLine(Guid.NewGuid(), 10, 5.00m);
        salesOrder.Confirm();
        salesOrderRepository.Seed(salesOrder);

        var handler = new CancelSalesOrderCommandHandler(salesOrderRepository, unitOfWork);

        var dto = await handler.HandleAsync(
            new CancelSalesOrderCommand { SalesOrderId = salesOrder.Id }, CancellationToken.None);

        Assert.Equal(SalesOrderStatus.Cancelled, dto.Status);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenSalesOrderDoesNotExist()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new CancelSalesOrderCommandHandler(salesOrderRepository, unitOfWork);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(
            new CancelSalesOrderCommand { SalesOrderId = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenOrderIsFullyFulfilled()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var productId = Guid.NewGuid();
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrder.AddLine(productId, 10, 5.00m);
        salesOrder.Confirm();
        salesOrder.FulfillProduct(productId, 10);
        salesOrderRepository.Seed(salesOrder);

        var handler = new CancelSalesOrderCommandHandler(salesOrderRepository, unitOfWork);

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(
            new CancelSalesOrderCommand { SalesOrderId = salesOrder.Id }, CancellationToken.None));
    }
}
