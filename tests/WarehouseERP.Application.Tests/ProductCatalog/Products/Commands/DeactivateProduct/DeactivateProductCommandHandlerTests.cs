using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.ProductCatalog.Products.Commands.DeactivateProduct;
using WarehouseERP.Application.Tests.ProductCatalog.Products.Fakes;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.Tests.ProductCatalog.Products.Commands.DeactivateProduct;

public class DeactivateProductCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_DeactivatesProduct_WhenProductExists()
    {
        var productRepository = new FakeProductRepository();
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        productRepository.Seed(product);

        var handler = new DeactivateProductCommandHandler(productRepository);

        var dto = await handler.HandleAsync(new DeactivateProductCommand { Id = product.Id }, CancellationToken.None);

        Assert.False(dto.IsActive);
        Assert.False(product.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        var productRepository = new FakeProductRepository();
        var handler = new DeactivateProductCommandHandler(productRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new DeactivateProductCommand { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToProductRepository()
    {
        var productRepository = new FakeProductRepository();
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        productRepository.Seed(product);

        var handler = new DeactivateProductCommandHandler(productRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new DeactivateProductCommand { Id = product.Id }, cts.Token);

        Assert.Equal(cts.Token, productRepository.LastCancellationToken);
    }
}
