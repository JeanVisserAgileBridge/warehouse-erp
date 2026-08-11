namespace WarehouseERP.Shared.Contracts.Customers;

public sealed class CreateCustomerRequest
{
    public required string Name { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
}
