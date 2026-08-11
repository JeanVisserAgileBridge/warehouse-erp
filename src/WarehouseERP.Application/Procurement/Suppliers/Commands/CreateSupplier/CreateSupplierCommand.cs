namespace WarehouseERP.Application.Procurement.Suppliers.Commands.CreateSupplier;

public sealed class CreateSupplierCommand
{
    public required string Name { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
}
