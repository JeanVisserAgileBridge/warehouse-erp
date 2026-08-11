using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Sales.Customers;

public sealed class CustomerDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
    public required bool IsActive { get; init; }

    public static CustomerDto FromDomain(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address,
            IsActive = customer.IsActive
        };
    }
}
