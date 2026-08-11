namespace WarehouseERP.Shared.Contracts.Suppliers;

public sealed class CreateSupplierRequest
{
    public required string Name { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
}
