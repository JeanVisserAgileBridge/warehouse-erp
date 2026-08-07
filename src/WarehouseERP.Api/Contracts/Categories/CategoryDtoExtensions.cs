using WarehouseERP.Shared.Contracts.Categories;
using ApplicationCategoryDto = WarehouseERP.Application.ProductCatalog.Categories.CategoryDto;

namespace WarehouseERP.Api.Contracts.Categories;

internal static class CategoryDtoExtensions
{
    public static CategoryDto ToContract(this ApplicationCategoryDto dto)
    {
        return new CategoryDto
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive
        };
    }

    public static IReadOnlyList<CategoryDto> ToContract(this IReadOnlyList<ApplicationCategoryDto> dtos)
    {
        return dtos.Select(ToContract).ToList();
    }
}
