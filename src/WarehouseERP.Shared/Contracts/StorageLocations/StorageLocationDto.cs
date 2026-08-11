namespace WarehouseERP.Shared.Contracts.StorageLocations;

public sealed class StorageLocationDto
{
    public required Guid Id { get; init; }
    public required Guid WarehouseId { get; init; }
    public required string Code { get; init; }
    public string? Description { get; init; }
    public required bool IsActive { get; init; }
}
