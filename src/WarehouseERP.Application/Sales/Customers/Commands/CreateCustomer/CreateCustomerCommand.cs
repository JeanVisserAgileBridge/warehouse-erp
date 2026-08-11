namespace WarehouseERP.Application.Sales.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommand
{
    public required string Name { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
}
