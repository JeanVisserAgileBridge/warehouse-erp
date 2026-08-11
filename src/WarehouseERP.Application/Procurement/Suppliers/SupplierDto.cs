using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Procurement.Suppliers;

public sealed class SupplierDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
    public required bool IsActive { get; init; }

    public static SupplierDto FromDomain(Supplier supplier)
    {
        return new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Email = supplier.Email,
            PhoneNumber = supplier.PhoneNumber,
            Address = supplier.Address,
            IsActive = supplier.IsActive
        };
    }
}
