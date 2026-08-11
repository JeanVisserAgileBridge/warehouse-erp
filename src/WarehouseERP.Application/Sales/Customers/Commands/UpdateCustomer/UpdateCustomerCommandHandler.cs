using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Sales.Customers.Commands.UpdateCustomer;

public sealed class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> HandleAsync(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Customer with id '{command.Id}' was not found.");

        var customerWithSameName = await _customerRepository.GetByNameAsync(command.Name, cancellationToken);
        if (customerWithSameName is not null && customerWithSameName.Id != customer.Id)
        {
            throw new DuplicateNameException($"A customer named '{command.Name}' already exists.");
        }

        customer.Rename(command.Name);
        customer.ChangeEmail(command.Email);
        customer.ChangePhoneNumber(command.PhoneNumber);
        customer.ChangeAddress(command.Address);

        await _customerRepository.UpdateAsync(customer, cancellationToken);

        return CustomerDto.FromDomain(customer);
    }
}
