using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.ProductCatalog.Products.Commands.CreateProduct;
using WarehouseERP.Application.Tests.ProductCatalog.Products.Fakes;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.Tests.ProductCatalog.Products.Commands.CreateProduct;

public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_AddsProductToRepository_WhenSkuIsUniqueAndCategoryIsActive()
    {
        var categoryRepository = new FakeCategoryRepository();
        var category = Category.Create("Beverages");
        categoryRepository.Seed(category);

        var productRepository = new FakeProductRepository();
        var handler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var command = new CreateProductCommand
        {
            Sku = "SKU-001",
            Name = "Widget",
            CategoryId = category.Id,
            UnitPrice = 9.99m
        };

        await handler.HandleAsync(command, CancellationToken.None);

        var products = await productRepository.GetAllAsync(CancellationToken.None);
        Assert.Single(products);
    }

    [Fact]
    public async Task HandleAsync_ReturnsMatchingProductDto_WhenValid()
    {
        var categoryRepository = new FakeCategoryRepository();
        var category = Category.Create("Beverages");
        categoryRepository.Seed(category);

        var productRepository = new FakeProductRepository();
        var handler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var command = new CreateProductCommand
        {
            Sku = "SKU-001",
            Name = "Widget",
            Description = "A useful widget",
            CategoryId = category.Id,
            UnitPrice = 9.99m
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("SKU-001", dto.Sku);
        Assert.Equal("Widget", dto.Name);
        Assert.Equal("A useful widget", dto.Description);
        Assert.Equal(category.Id, dto.CategoryId);
        Assert.Equal(9.99m, dto.UnitPrice);
        Assert.True(dto.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateSkuException_WhenSkuAlreadyExistsWithDifferentCase()
    {
        var categoryRepository = new FakeCategoryRepository();
        var category = Category.Create("Beverages");
        categoryRepository.Seed(category);

        var productRepository = new FakeProductRepository();
        productRepository.Seed(Product.Create("sku-001", "Existing Widget", category.Id, 5.00m));

        var handler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var command = new CreateProductCommand
        {
            Sku = "SKU-001",
            Name = "Widget",
            CategoryId = category.Id,
            UnitPrice = 9.99m
        };

        await Assert.ThrowsAsync<DuplicateSkuException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenCategoryDoesNotExist()
    {
        var categoryRepository = new FakeCategoryRepository();
        var productRepository = new FakeProductRepository();
        var handler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var command = new CreateProductCommand
        {
            Sku = "SKU-001",
            Name = "Widget",
            CategoryId = Guid.NewGuid(),
            UnitPrice = 9.99m
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsInactiveCategoryException_WhenCategoryIsInactive()
    {
        var categoryRepository = new FakeCategoryRepository();
        var category = Category.Create("Beverages");
        category.Deactivate();
        categoryRepository.Seed(category);

        var productRepository = new FakeProductRepository();
        var handler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var command = new CreateProductCommand
        {
            Sku = "SKU-001",
            Name = "Widget",
            CategoryId = category.Id,
            UnitPrice = 9.99m
        };

        await Assert.ThrowsAsync<InactiveCategoryException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToRepositories()
    {
        var categoryRepository = new FakeCategoryRepository();
        var category = Category.Create("Beverages");
        categoryRepository.Seed(category);

        var productRepository = new FakeProductRepository();
        var handler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var command = new CreateProductCommand
        {
            Sku = "SKU-001",
            Name = "Widget",
            CategoryId = category.Id,
            UnitPrice = 9.99m
        };

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(command, cts.Token);

        Assert.Equal(cts.Token, productRepository.LastCancellationToken);
        Assert.Equal(cts.Token, categoryRepository.LastCancellationToken);
    }
}
