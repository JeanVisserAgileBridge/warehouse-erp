namespace WarehouseERP.Shared.Contracts.Suppliers;

public sealed class SupplierDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
    public required bool IsActive { get; init; }
}
