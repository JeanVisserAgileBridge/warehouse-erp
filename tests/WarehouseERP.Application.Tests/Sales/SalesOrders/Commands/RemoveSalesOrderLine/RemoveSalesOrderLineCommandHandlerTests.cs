using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.SalesOrders.Commands.RemoveSalesOrderLine;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Sales.SalesOrders.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.SalesOrders.Commands.RemoveSalesOrderLine;

public class RemoveSalesOrderLineCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_RemovesLine_WhenOrderIsDraft()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var productId = Guid.NewGuid();
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrder.AddLine(productId, 10, 5.00m);
        salesOrderRepository.Seed(salesOrder);

        var handler = new RemoveSalesOrderLineCommandHandler(salesOrderRepository, unitOfWork);

        var dto = await handler.HandleAsync(
            new RemoveSalesOrderLineCommand { SalesOrderId = salesOrder.Id, ProductId = productId },
            CancellationToken.None);

        Assert.Empty(dto.Lines);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenSalesOrderDoesNotExist()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new RemoveSalesOrderLineCommandHandler(salesOrderRepository, unitOfWork);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(
            new RemoveSalesOrderLineCommand { SalesOrderId = Guid.NewGuid(), ProductId = Guid.NewGuid() },
            CancellationToken.None));
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

        var handler = new RemoveSalesOrderLineCommandHandler(salesOrderRepository, unitOfWork);

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(
            new RemoveSalesOrderLineCommand { SalesOrderId = salesOrder.Id, ProductId = productId },
            CancellationToken.None));
    }
}
