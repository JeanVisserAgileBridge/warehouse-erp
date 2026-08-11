using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.Customers.Commands.ActivateCustomer;
using WarehouseERP.Application.Tests.Sales.Customers.Fakes;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.Customers.Commands.ActivateCustomer;

public class ActivateCustomerCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ActivatesCustomer_WhenCustomerExists()
    {
        var customerRepository = new FakeCustomerRepository();
        var customer = Customer.Create("Jane Doe");
        customer.Deactivate();
        customerRepository.Seed(customer);

        var handler = new ActivateCustomerCommandHandler(customerRepository);

        var dto = await handler.HandleAsync(new ActivateCustomerCommand { Id = customer.Id }, CancellationToken.None);

        Assert.True(dto.IsActive);
        Assert.True(customer.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenCustomerDoesNotExist()
    {
        var customerRepository = new FakeCustomerRepository();
        var handler = new ActivateCustomerCommandHandler(customerRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new ActivateCustomerCommand { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToCustomerRepository()
    {
        var customerRepository = new FakeCustomerRepository();
        var customer = Customer.Create("Jane Doe");
        customerRepository.Seed(customer);

        var handler = new ActivateCustomerCommandHandler(customerRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new ActivateCustomerCommand { Id = customer.Id }, cts.Token);

        Assert.Equal(cts.Token, customerRepository.LastCancellationToken);
    }
}
