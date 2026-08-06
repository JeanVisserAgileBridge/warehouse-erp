using WarehouseERP.Application.ProductCatalog.Products;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.Tests.ProductCatalog.Products;

public class ProductDtoTests
{
    [Fact]
    public void FromDomain_MapsAllPropertiesFromProduct()
    {
        var categoryId = Guid.NewGuid();
        var product = Product.Create("SKU-001", "Widget", categoryId, 9.99m, "A useful widget");

        var dto = ProductDto.FromDomain(product);

        Assert.Equal(product.Id, dto.Id);
        Assert.Equal(product.Sku, dto.Sku);
        Assert.Equal(product.Name, dto.Name);
        Assert.Equal(product.Description, dto.Description);
        Assert.Equal(product.CategoryId, dto.CategoryId);
        Assert.Equal(product.UnitPrice, dto.UnitPrice);
        Assert.Equal(product.IsActive, dto.IsActive);
        Assert.Equal(product.CreatedAt, dto.CreatedAt);
        Assert.Equal(product.UpdatedAt, dto.UpdatedAt);
    }
}
