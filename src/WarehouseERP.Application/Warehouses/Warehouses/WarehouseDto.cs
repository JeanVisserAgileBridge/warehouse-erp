using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Warehouses.Warehouses;

public sealed class WarehouseDto
{
    public required Guid Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? Address { get; init; }
    public required bool IsActive { get; init; }

    public static WarehouseDto FromDomain(Warehouse warehouse)
    {
        return new WarehouseDto
        {
            Id = warehouse.Id,
            Code = warehouse.Code,
            Name = warehouse.Name,
            Address = warehouse.Address,
            IsActive = warehouse.IsActive
        };
    }
}
