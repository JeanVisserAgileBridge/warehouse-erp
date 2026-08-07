namespace WarehouseERP.Shared.Contracts.Categories;

public sealed class CategoryDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required bool IsActive { get; init; }
}
