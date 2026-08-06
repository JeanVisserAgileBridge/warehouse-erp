using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.ProductCatalog.Products.Queries.GetProductById;
using WarehouseERP.Application.Tests.ProductCatalog.Products.Fakes;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.Tests.ProductCatalog.Products.Queries.GetProductById;

public class GetProductByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsMatchingProductDto_WhenProductExists()
    {
        var productRepository = new FakeProductRepository();
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m, "A useful widget");
        productRepository.Seed(product);

        var handler = new GetProductByIdQueryHandler(productRepository);

        var dto = await handler.HandleAsync(new GetProductByIdQuery { Id = product.Id }, CancellationToken.None);

        Assert.Equal(product.Id, dto.Id);
        Assert.Equal(product.Sku, dto.Sku);
        Assert.Equal(product.Name, dto.Name);
        Assert.Equal(product.Description, dto.Description);
        Assert.Equal(product.CategoryId, dto.CategoryId);
        Assert.Equal(product.UnitPrice, dto.UnitPrice);
        Assert.Equal(product.IsActive, dto.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        var productRepository = new FakeProductRepository();
        var handler = new GetProductByIdQueryHandler(productRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new GetProductByIdQuery { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToProductRepository()
    {
        var productRepository = new FakeProductRepository();
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        productRepository.Seed(product);

        var handler = new GetProductByIdQueryHandler(productRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new GetProductByIdQuery { Id = product.Id }, cts.Token);

        Assert.Equal(cts.Token, productRepository.LastCancellationToken);
    }
}
