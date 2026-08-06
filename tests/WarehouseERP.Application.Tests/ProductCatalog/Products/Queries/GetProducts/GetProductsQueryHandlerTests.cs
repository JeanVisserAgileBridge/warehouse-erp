using WarehouseERP.Application.ProductCatalog.Products.Queries.GetProducts;
using WarehouseERP.Application.Tests.ProductCatalog.Products.Fakes;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.Tests.ProductCatalog.Products.Queries.GetProducts;

public class GetProductsQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsAllProductsAsDtos()
    {
        var productRepository = new FakeProductRepository();
        var categoryId = Guid.NewGuid();
        var first = Product.Create("SKU-001", "Widget", categoryId, 9.99m);
        var second = Product.Create("SKU-002", "Gadget", categoryId, 14.99m);
        productRepository.Seed(first);
        productRepository.Seed(second);

        var handler = new GetProductsQueryHandler(productRepository);

        var dtos = await handler.HandleAsync(new GetProductsQuery(), CancellationToken.None);

        Assert.Equal(2, dtos.Count);
        Assert.Contains(dtos, d => d.Sku == "SKU-001");
        Assert.Contains(dtos, d => d.Sku == "SKU-002");
    }

    [Fact]
    public async Task HandleAsync_ReturnsEmptyList_WhenNoProductsExist()
    {
        var productRepository = new FakeProductRepository();
        var handler = new GetProductsQueryHandler(productRepository);

        var dtos = await handler.HandleAsync(new GetProductsQuery(), CancellationToken.None);

        Assert.Empty(dtos);
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToProductRepository()
    {
        var productRepository = new FakeProductRepository();
        var handler = new GetProductsQueryHandler(productRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new GetProductsQuery(), cts.Token);

        Assert.Equal(cts.Token, productRepository.LastCancellationToken);
    }
}
