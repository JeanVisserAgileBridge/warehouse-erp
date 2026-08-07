namespace WarehouseERP.Api.Contracts.Categories;

public sealed class CreateCategoryRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}
