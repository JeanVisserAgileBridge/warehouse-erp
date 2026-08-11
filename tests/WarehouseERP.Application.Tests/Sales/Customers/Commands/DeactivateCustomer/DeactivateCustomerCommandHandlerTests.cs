using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.Customers.Commands.DeactivateCustomer;
using WarehouseERP.Application.Tests.Sales.Customers.Fakes;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.Customers.Commands.DeactivateCustomer;

public class DeactivateCustomerCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_DeactivatesCustomer_WhenCustomerExists()
    {
        var customerRepository = new FakeCustomerRepository();
        var customer = Customer.Create("Jane Doe");
        customerRepository.Seed(customer);

        var handler = new DeactivateCustomerCommandHandler(customerRepository);

        var dto = await handler.HandleAsync(new DeactivateCustomerCommand { Id = customer.Id }, CancellationToken.None);

        Assert.False(dto.IsActive);
        Assert.False(customer.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenCustomerDoesNotExist()
    {
        var customerRepository = new FakeCustomerRepository();
        var handler = new DeactivateCustomerCommandHandler(customerRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new DeactivateCustomerCommand { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToCustomerRepository()
    {
        var customerRepository = new FakeCustomerRepository();
        var customer = Customer.Create("Jane Doe");
        customerRepository.Seed(customer);

        var handler = new DeactivateCustomerCommandHandler(customerRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new DeactivateCustomerCommand { Id = customer.Id }, cts.Token);

        Assert.Equal(cts.Token, customerRepository.LastCancellationToken);
    }
}
