using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.SalesOrders.Commands.CreateSalesOrder;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Sales.Customers.Fakes;
using WarehouseERP.Application.Tests.Sales.SalesOrders.Fakes;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.SalesOrders.Commands.CreateSalesOrder;

public class CreateSalesOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_CreatesSalesOrder_WhenCustomerExistsAndIsActive()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var customerRepository = new FakeCustomerRepository();
        var unitOfWork = new FakeUnitOfWork();

        var customer = Customer.Create("Acme Retail");
        customerRepository.Seed(customer);

        var handler = new CreateSalesOrderCommandHandler(salesOrderRepository, customerRepository, unitOfWork);

        var command = new CreateSalesOrderCommand
        {
            CustomerId = customer.Id,
            OrderNumber = "SO-001",
            OrderDate = DateTime.UtcNow
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("SO-001", dto.OrderNumber);
        Assert.Equal(SalesOrderStatus.Draft, dto.Status);
        Assert.Equal(1, salesOrderRepository.AddCallCount);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenCustomerDoesNotExist()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var customerRepository = new FakeCustomerRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new CreateSalesOrderCommandHandler(salesOrderRepository, customerRepository, unitOfWork);

        var command = new CreateSalesOrderCommand
        {
            CustomerId = Guid.NewGuid(),
            OrderNumber = "SO-001",
            OrderDate = DateTime.UtcNow
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsInactiveCustomerException_WhenCustomerIsNotActive()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var customerRepository = new FakeCustomerRepository();
        var unitOfWork = new FakeUnitOfWork();

        var customer = Customer.Create("Acme Retail");
        customer.Deactivate();
        customerRepository.Seed(customer);

        var handler = new CreateSalesOrderCommandHandler(salesOrderRepository, customerRepository, unitOfWork);

        var command = new CreateSalesOrderCommand
        {
            CustomerId = customer.Id,
            OrderNumber = "SO-001",
            OrderDate = DateTime.UtcNow
        };

        await Assert.ThrowsAsync<InactiveCustomerException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateOrderNumberException_WhenOrderNumberAlreadyExists()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var customerRepository = new FakeCustomerRepository();
        var unitOfWork = new FakeUnitOfWork();

        var customer = Customer.Create("Acme Retail");
        customerRepository.Seed(customer);
        salesOrderRepository.Seed(SalesOrder.Create(customer.Id, "SO-001", DateTime.UtcNow));

        var handler = new CreateSalesOrderCommandHandler(salesOrderRepository, customerRepository, unitOfWork);

        var command = new CreateSalesOrderCommand
        {
            CustomerId = customer.Id,
            OrderNumber = "so-001",
            OrderDate = DateTime.UtcNow
        };

        await Assert.ThrowsAsync<DuplicateOrderNumberException>(() => handler.HandleAsync(command, CancellationToken.None));
    }
}
