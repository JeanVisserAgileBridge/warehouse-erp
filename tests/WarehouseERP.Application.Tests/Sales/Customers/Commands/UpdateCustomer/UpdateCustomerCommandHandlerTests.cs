using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.Customers.Commands.UpdateCustomer;
using WarehouseERP.Application.Tests.Sales.Customers.Fakes;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_UpdatesCustomer_WhenValid()
    {
        var customerRepository = new FakeCustomerRepository();
        var customer = Customer.Create("Jane Doe");
        customerRepository.Seed(customer);

        var handler = new UpdateCustomerCommandHandler(customerRepository);

        var command = new UpdateCustomerCommand
        {
            Id = customer.Id,
            Name = "Jane Doe Updated",
            Email = "updated@example.test",
            PhoneNumber = "555-0199",
            Address = "2 Main Street"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("Jane Doe Updated", dto.Name);
        Assert.Equal("updated@example.test", dto.Email);
        Assert.Equal("555-0199", dto.PhoneNumber);
        Assert.Equal("2 Main Street", dto.Address);
        Assert.Equal("Jane Doe Updated", customer.Name);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenCustomerDoesNotExist()
    {
        var customerRepository = new FakeCustomerRepository();
        var handler = new UpdateCustomerCommandHandler(customerRepository);

        var command = new UpdateCustomerCommand
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe"
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateNameException_WhenNameBelongsToAnotherCustomer()
    {
        var customerRepository = new FakeCustomerRepository();
        var customerToUpdate = Customer.Create("Jane Doe");
        var otherCustomer = Customer.Create("John Smith");
        customerRepository.Seed(customerToUpdate);
        customerRepository.Seed(otherCustomer);

        var handler = new UpdateCustomerCommandHandler(customerRepository);

        var command = new UpdateCustomerCommand
        {
            Id = customerToUpdate.Id,
            Name = "john smith"
        };

        await Assert.ThrowsAsync<DuplicateNameException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_AllowsUpdate_WhenNameIsUnchanged()
    {
        var customerRepository = new FakeCustomerRepository();
        var customer = Customer.Create("Jane Doe");
        customerRepository.Seed(customer);

        var handler = new UpdateCustomerCommandHandler(customerRepository);

        var command = new UpdateCustomerCommand
        {
            Id = customer.Id,
            Name = "Jane Doe",
            PhoneNumber = "555-0199"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("Jane Doe", dto.Name);
        Assert.Equal("555-0199", dto.PhoneNumber);
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToCustomerRepository()
    {
        var customerRepository = new FakeCustomerRepository();
        var customer = Customer.Create("Jane Doe");
        customerRepository.Seed(customer);

        var handler = new UpdateCustomerCommandHandler(customerRepository);

        var command = new UpdateCustomerCommand
        {
            Id = customer.Id,
            Name = "Jane Doe"
        };

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(command, cts.Token);

        Assert.Equal(cts.Token, customerRepository.LastCancellationToken);
    }
}
