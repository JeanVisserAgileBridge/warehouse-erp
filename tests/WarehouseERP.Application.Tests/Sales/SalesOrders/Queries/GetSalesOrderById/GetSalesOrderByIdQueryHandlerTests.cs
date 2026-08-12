using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.SalesOrders.Queries.GetSalesOrderById;
using WarehouseERP.Application.Tests.Sales.SalesOrders.Fakes;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.SalesOrders.Queries.GetSalesOrderById;

public class GetSalesOrderByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsSalesOrder_WhenItExists()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrderRepository.Seed(salesOrder);

        var handler = new GetSalesOrderByIdQueryHandler(salesOrderRepository);

        var dto = await handler.HandleAsync(new GetSalesOrderByIdQuery { Id = salesOrder.Id }, CancellationToken.None);

        Assert.Equal(salesOrder.Id, dto.Id);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenSalesOrderDoesNotExist()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var handler = new GetSalesOrderByIdQueryHandler(salesOrderRepository);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(
            new GetSalesOrderByIdQuery { Id = Guid.NewGuid() }, CancellationToken.None));
    }
}
