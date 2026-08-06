namespace WarehouseERP.Application.ProductCatalog.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommand
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}
