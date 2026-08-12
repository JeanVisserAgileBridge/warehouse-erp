using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseERP.Api.Contracts.Customers;
using WarehouseERP.Api.DependencyInjection;
using WarehouseERP.Application.Common;
using WarehouseERP.Application.Sales.Customers.Commands.ActivateCustomer;
using WarehouseERP.Application.Sales.Customers.Commands.CreateCustomer;
using WarehouseERP.Application.Sales.Customers.Commands.DeactivateCustomer;
using WarehouseERP.Application.Sales.Customers.Commands.UpdateCustomer;
using WarehouseERP.Application.Sales.Customers.Queries.GetCustomerById;
using WarehouseERP.Application.Sales.Customers.Queries.GetCustomers;
using WarehouseERP.Shared.Contracts.Customers;
using ApplicationCustomerDto = WarehouseERP.Application.Sales.Customers.CustomerDto;

namespace WarehouseERP.Api.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize(Policy = PolicyNames.SalesAccess)]
public sealed class CustomersController : ControllerBase
{
    private readonly IQueryHandler<GetCustomersQuery, IReadOnlyList<ApplicationCustomerDto>> _getCustomers;
    private readonly IQueryHandler<GetCustomerByIdQuery, ApplicationCustomerDto> _getCustomerById;
    private readonly ICommandHandler<CreateCustomerCommand, ApplicationCustomerDto> _createCustomer;
    private readonly ICommandHandler<UpdateCustomerCommand, ApplicationCustomerDto> _updateCustomer;
    private readonly ICommandHandler<ActivateCustomerCommand, ApplicationCustomerDto> _activateCustomer;
    private readonly ICommandHandler<DeactivateCustomerCommand, ApplicationCustomerDto> _deactivateCustomer;

    public CustomersController(
        IQueryHandler<GetCustomersQuery, IReadOnlyList<ApplicationCustomerDto>> getCustomers,
        IQueryHandler<GetCustomerByIdQuery, ApplicationCustomerDto> getCustomerById,
        ICommandHandler<CreateCustomerCommand, ApplicationCustomerDto> createCustomer,
        ICommandHandler<UpdateCustomerCommand, ApplicationCustomerDto> updateCustomer,
        ICommandHandler<ActivateCustomerCommand, ApplicationCustomerDto> activateCustomer,
        ICommandHandler<DeactivateCustomerCommand, ApplicationCustomerDto> deactivateCustomer)
    {
        _getCustomers = getCustomers;
        _getCustomerById = getCustomerById;
        _createCustomer = createCustomer;
        _updateCustomer = updateCustomer;
        _activateCustomer = activateCustomer;
        _deactivateCustomer = deactivateCustomer;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CustomerDto>>> GetAll(CancellationToken cancellationToken)
    {
        var customers = await _getCustomers.HandleAsync(new GetCustomersQuery(), cancellationToken);

        return Ok(customers.ToContract());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _getCustomerById.HandleAsync(new GetCustomerByIdQuery { Id = id }, cancellationToken);

        return Ok(customer.ToContract());
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCustomerCommand
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address
        };

        var customer = await _createCustomer.HandleAsync(command, cancellationToken);
        var contract = customer.ToContract();

        return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CustomerDto>> Update(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCustomerCommand
        {
            Id = id,
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address
        };

        var customer = await _updateCustomer.HandleAsync(command, cancellationToken);

        return Ok(customer.ToContract());
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<ActionResult<CustomerDto>> Activate(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _activateCustomer.HandleAsync(new ActivateCustomerCommand { Id = id }, cancellationToken);

        return Ok(customer.ToContract());
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<ActionResult<CustomerDto>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _deactivateCustomer.HandleAsync(new DeactivateCustomerCommand { Id = id }, cancellationToken);

        return Ok(customer.ToContract());
    }
}
