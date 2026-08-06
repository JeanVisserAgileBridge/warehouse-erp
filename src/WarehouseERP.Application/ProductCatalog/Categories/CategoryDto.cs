using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.ProductCatalog.Categories;

public sealed class CategoryDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required bool IsActive { get; init; }

    public static CategoryDto FromDomain(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive
        };
    }
}
