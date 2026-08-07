using WarehouseERP.Shared.Contracts.Products;
using ApplicationProductDto = WarehouseERP.Application.ProductCatalog.Products.ProductDto;

namespace WarehouseERP.Api.Contracts.Products;

internal static class ProductDtoExtensions
{
    public static ProductDto ToContract(this ApplicationProductDto dto)
    {
        return new ProductDto
        {
            Id = dto.Id,
            Sku = dto.Sku,
            Name = dto.Name,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            UnitPrice = dto.UnitPrice,
            IsActive = dto.IsActive,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public static IReadOnlyList<ProductDto> ToContract(this IReadOnlyList<ApplicationProductDto> dtos)
    {
        return dtos.Select(ToContract).ToList();
    }
}
