using WarehouseERP.Application.Sales.SalesOrders.Queries.GetSalesOrdersByCustomerId;
using WarehouseERP.Application.Tests.Sales.SalesOrders.Fakes;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.SalesOrders.Queries.GetSalesOrdersByCustomerId;

public class GetSalesOrdersByCustomerIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsOnlySalesOrdersForGivenCustomer()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var customerId = Guid.NewGuid();
        salesOrderRepository.Seed(SalesOrder.Create(customerId, "SO-001", DateTime.UtcNow));
        salesOrderRepository.Seed(SalesOrder.Create(Guid.NewGuid(), "SO-002", DateTime.UtcNow));

        var handler = new GetSalesOrdersByCustomerIdQueryHandler(salesOrderRepository);

        var result = await handler.HandleAsync(
            new GetSalesOrdersByCustomerIdQuery { CustomerId = customerId }, CancellationToken.None);

        var salesOrder = Assert.Single(result);
        Assert.Equal("SO-001", salesOrder.OrderNumber);
    }
}
