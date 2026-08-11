using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.Customers.Queries.GetCustomerById;
using WarehouseERP.Application.Tests.Sales.Customers.Fakes;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsMatchingCustomerDto_WhenCustomerExists()
    {
        var customerRepository = new FakeCustomerRepository();
        var customer = Customer.Create("Jane Doe", "jane@example.test", "555-0100", "1 Main Street");
        customerRepository.Seed(customer);

        var handler = new GetCustomerByIdQueryHandler(customerRepository);

        var dto = await handler.HandleAsync(new GetCustomerByIdQuery { Id = customer.Id }, CancellationToken.None);

        Assert.Equal(customer.Id, dto.Id);
        Assert.Equal(customer.Name, dto.Name);
        Assert.Equal(customer.Email, dto.Email);
        Assert.Equal(customer.PhoneNumber, dto.PhoneNumber);
        Assert.Equal(customer.Address, dto.Address);
        Assert.Equal(customer.IsActive, dto.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenCustomerDoesNotExist()
    {
        var customerRepository = new FakeCustomerRepository();
        var handler = new GetCustomerByIdQueryHandler(customerRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new GetCustomerByIdQuery { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToCustomerRepository()
    {
        var customerRepository = new FakeCustomerRepository();
        var customer = Customer.Create("Jane Doe");
        customerRepository.Seed(customer);

        var handler = new GetCustomerByIdQueryHandler(customerRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new GetCustomerByIdQuery { Id = customer.Id }, cts.Token);

        Assert.Equal(cts.Token, customerRepository.LastCancellationToken);
    }
}
