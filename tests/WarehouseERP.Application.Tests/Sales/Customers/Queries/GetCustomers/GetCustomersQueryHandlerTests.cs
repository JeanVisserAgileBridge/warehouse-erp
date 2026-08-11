using WarehouseERP.Application.Sales.Customers.Queries.GetCustomers;
using WarehouseERP.Application.Tests.Sales.Customers.Fakes;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsAllCustomersAsDtos()
    {
        var customerRepository = new FakeCustomerRepository();
        var first = Customer.Create("Jane Doe");
        var second = Customer.Create("John Smith");
        customerRepository.Seed(first);
        customerRepository.Seed(second);

        var handler = new GetCustomersQueryHandler(customerRepository);

        var dtos = await handler.HandleAsync(new GetCustomersQuery(), CancellationToken.None);

        Assert.Equal(2, dtos.Count);
        Assert.Contains(dtos, d => d.Name == "Jane Doe");
        Assert.Contains(dtos, d => d.Name == "John Smith");
    }

    [Fact]
    public async Task HandleAsync_ReturnsEmptyList_WhenNoCustomersExist()
    {
        var customerRepository = new FakeCustomerRepository();
        var handler = new GetCustomersQueryHandler(customerRepository);

        var dtos = await handler.HandleAsync(new GetCustomersQuery(), CancellationToken.None);

        Assert.Empty(dtos);
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToCustomerRepository()
    {
        var customerRepository = new FakeCustomerRepository();
        var handler = new GetCustomersQueryHandler(customerRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new GetCustomersQuery(), cts.Token);

        Assert.Equal(cts.Token, customerRepository.LastCancellationToken);
    }
}
