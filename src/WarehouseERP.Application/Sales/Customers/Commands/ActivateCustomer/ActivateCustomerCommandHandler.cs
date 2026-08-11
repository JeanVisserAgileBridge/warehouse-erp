using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Sales.Customers.Commands.ActivateCustomer;

public sealed class ActivateCustomerCommandHandler : ICommandHandler<ActivateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public ActivateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> HandleAsync(ActivateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Customer with id '{command.Id}' was not found.");

        customer.Activate();

        await _customerRepository.UpdateAsync(customer, cancellationToken);

        return CustomerDto.FromDomain(customer);
    }
}
