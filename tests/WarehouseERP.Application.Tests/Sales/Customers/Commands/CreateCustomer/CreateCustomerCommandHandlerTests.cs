using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.Customers.Commands.CreateCustomer;
using WarehouseERP.Application.Tests.Sales.Customers.Fakes;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_AddsCustomerToRepository_WhenNameIsUnique()
    {
        var customerRepository = new FakeCustomerRepository();
        var handler = new CreateCustomerCommandHandler(customerRepository);

        var command = new CreateCustomerCommand
        {
            Name = "Jane Doe"
        };

        await handler.HandleAsync(command, CancellationToken.None);

        var customers = await customerRepository.GetAllAsync(CancellationToken.None);
        Assert.Single(customers);
    }

    [Fact]
    public async Task HandleAsync_ReturnsMatchingCustomerDto_WhenValid()
    {
        var customerRepository = new FakeCustomerRepository();
        var handler = new CreateCustomerCommandHandler(customerRepository);

        var command = new CreateCustomerCommand
        {
            Name = "Jane Doe",
            Email = "jane@example.test",
            PhoneNumber = "555-0100",
            Address = "1 Main Street"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("Jane Doe", dto.Name);
        Assert.Equal("jane@example.test", dto.Email);
        Assert.Equal("555-0100", dto.PhoneNumber);
        Assert.Equal("1 Main Street", dto.Address);
        Assert.True(dto.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateNameException_WhenNameAlreadyExistsWithDifferentCase()
    {
        var customerRepository = new FakeCustomerRepository();
        customerRepository.Seed(Customer.Create("jane doe"));

        var handler = new CreateCustomerCommandHandler(customerRepository);

        var command = new CreateCustomerCommand
        {
            Name = "Jane Doe"
        };

        await Assert.ThrowsAsync<DuplicateNameException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToCustomerRepository()
    {
        var customerRepository = new FakeCustomerRepository();
        var handler = new CreateCustomerCommandHandler(customerRepository);

        var command = new CreateCustomerCommand
        {
            Name = "Jane Doe"
        };

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(command, cts.Token);

        Assert.Equal(cts.Token, customerRepository.LastCancellationToken);
    }
}
