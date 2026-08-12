using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.Customers;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Sales.SalesOrders.Commands.CreateSalesOrder;

public sealed class CreateSalesOrderCommandHandler : ICommandHandler<CreateSalesOrderCommand, SalesOrderDto>
{
    private readonly ISalesOrderRepository _salesOrderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSalesOrderCommandHandler(
        ISalesOrderRepository salesOrderRepository,
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _salesOrderRepository = salesOrderRepository;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SalesOrderDto> HandleAsync(CreateSalesOrderCommand command, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(command.CustomerId, cancellationToken)
            ?? throw new NotFoundException($"Customer with id '{command.CustomerId}' was not found.");

        if (!customer.IsActive)
        {
            throw new InactiveCustomerException($"Customer with id '{command.CustomerId}' is not active.");
        }

        var existingOrder = await _salesOrderRepository.GetByOrderNumberAsync(command.OrderNumber, cancellationToken);
        if (existingOrder is not null)
        {
            throw new DuplicateOrderNumberException($"A sales order numbered '{command.OrderNumber}' already exists.");
        }

        var salesOrder = SalesOrder.Create(command.CustomerId, command.OrderNumber, command.OrderDate, command.Notes);

        await _salesOrderRepository.AddAsync(salesOrder, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return SalesOrderDto.FromDomain(salesOrder);
    }
}
