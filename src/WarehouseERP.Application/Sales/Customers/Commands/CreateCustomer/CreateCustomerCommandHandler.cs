using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Sales.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> HandleAsync(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var existingCustomer = await _customerRepository.GetByNameAsync(command.Name, cancellationToken);
        if (existingCustomer is not null)
        {
            throw new DuplicateNameException($"A customer named '{command.Name}' already exists.");
        }

        var customer = Customer.Create(command.Name, command.Email, command.PhoneNumber, command.Address);

        await _customerRepository.AddAsync(customer, cancellationToken);

        return CustomerDto.FromDomain(customer);
    }
}
