using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.ProductCatalog.Products.Commands.ActivateProduct;
using WarehouseERP.Application.Tests.ProductCatalog.Products.Fakes;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.Tests.ProductCatalog.Products.Commands.ActivateProduct;

public class ActivateProductCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ActivatesProduct_WhenProductExists()
    {
        var productRepository = new FakeProductRepository();
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        product.Deactivate();
        productRepository.Seed(product);

        var handler = new ActivateProductCommandHandler(productRepository);

        var dto = await handler.HandleAsync(new ActivateProductCommand { Id = product.Id }, CancellationToken.None);

        Assert.True(dto.IsActive);
        Assert.True(product.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        var productRepository = new FakeProductRepository();
        var handler = new ActivateProductCommandHandler(productRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new ActivateProductCommand { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToProductRepository()
    {
        var productRepository = new FakeProductRepository();
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        productRepository.Seed(product);

        var handler = new ActivateProductCommandHandler(productRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new ActivateProductCommand { Id = product.Id }, cts.Token);

        Assert.Equal(cts.Token, productRepository.LastCancellationToken);
    }
}
