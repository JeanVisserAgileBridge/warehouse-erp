namespace WarehouseERP.Application.ProductCatalog.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommand
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}
