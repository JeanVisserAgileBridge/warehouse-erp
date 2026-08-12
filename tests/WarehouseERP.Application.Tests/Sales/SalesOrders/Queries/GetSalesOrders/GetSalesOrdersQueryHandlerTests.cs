using WarehouseERP.Application.Sales.SalesOrders.Queries.GetSalesOrders;
using WarehouseERP.Application.Tests.Sales.SalesOrders.Fakes;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.SalesOrders.Queries.GetSalesOrders;

public class GetSalesOrdersQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsAllSalesOrders()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        salesOrderRepository.Seed(SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow));
        salesOrderRepository.Seed(SalesOrder.Create(Guid.NewGuid(), "SO-002", DateTime.UtcNow));

        var handler = new GetSalesOrdersQueryHandler(salesOrderRepository);

        var result = await handler.HandleAsync(new GetSalesOrdersQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
    }
}
