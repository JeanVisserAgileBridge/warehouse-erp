namespace WarehouseERP.Application.Sales.Customers.Commands.UpdateCustomer;

public sealed class UpdateCustomerCommand
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
}
