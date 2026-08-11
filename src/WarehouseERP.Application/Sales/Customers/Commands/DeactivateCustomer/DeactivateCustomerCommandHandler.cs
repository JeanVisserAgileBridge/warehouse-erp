using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Sales.Customers.Commands.DeactivateCustomer;

public sealed class DeactivateCustomerCommandHandler : ICommandHandler<DeactivateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public DeactivateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> HandleAsync(DeactivateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Customer with id '{command.Id}' was not found.");

        customer.Deactivate();

        await _customerRepository.UpdateAsync(customer, cancellationToken);

        return CustomerDto.FromDomain(customer);
    }
}
