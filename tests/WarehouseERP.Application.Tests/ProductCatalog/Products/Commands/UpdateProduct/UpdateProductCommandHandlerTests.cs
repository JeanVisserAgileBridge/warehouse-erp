using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.ProductCatalog.Products.Commands.UpdateProduct;
using WarehouseERP.Application.Tests.ProductCatalog.Products.Fakes;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.Tests.ProductCatalog.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_UpdatesProduct_WhenValid()
    {
        var categoryRepository = new FakeCategoryRepository();
        var category = Category.Create("Beverages");
        categoryRepository.Seed(category);

        var productRepository = new FakeProductRepository();
        var product = Product.Create("SKU-001", "Widget", category.Id, 9.99m);
        productRepository.Seed(product);

        var handler = new UpdateProductCommandHandler(productRepository, categoryRepository);

        var command = new UpdateProductCommand
        {
            Id = product.Id,
            Sku = "SKU-001",
            Name = "Updated Widget",
            Description = "Updated description",
            CategoryId = category.Id,
            UnitPrice = 12.50m
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("Updated Widget", dto.Name);
        Assert.Equal("Updated description", dto.Description);
        Assert.Equal(12.50m, dto.UnitPrice);
        Assert.Equal("Updated Widget", product.Name);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        var categoryRepository = new FakeCategoryRepository();
        var productRepository = new FakeProductRepository();
        var handler = new UpdateProductCommandHandler(productRepository, categoryRepository);

        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Sku = "SKU-001",
            Name = "Widget",
            CategoryId = Guid.NewGuid(),
            UnitPrice = 9.99m
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateSkuException_WhenSkuBelongsToAnotherProduct()
    {
        var categoryRepository = new FakeCategoryRepository();
        var category = Category.Create("Beverages");
        categoryRepository.Seed(category);

        var productRepository = new FakeProductRepository();
        var productToUpdate = Product.Create("SKU-001", "Widget", category.Id, 9.99m);
        var otherProduct = Product.Create("SKU-002", "Gadget", category.Id, 14.99m);
        productRepository.Seed(productToUpdate);
        productRepository.Seed(otherProduct);

        var handler = new UpdateProductCommandHandler(productRepository, categoryRepository);

        var command = new UpdateProductCommand
        {
            Id = productToUpdate.Id,
            Sku = "sku-002",
            Name = "Widget",
            CategoryId = category.Id,
            UnitPrice = 9.99m
        };

        await Assert.ThrowsAsync<DuplicateSkuException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_AllowsUpdate_WhenSkuIsUnchanged()
    {
        var categoryRepository = new FakeCategoryRepository();
        var category = Category.Create("Beverages");
        categoryRepository.Seed(category);

        var productRepository = new FakeProductRepository();
        var product = Product.Create("SKU-001", "Widget", category.Id, 9.99m);
        productRepository.Seed(product);

        var handler = new UpdateProductCommandHandler(productRepository, categoryRepository);

        var command = new UpdateProductCommand
        {
            Id = product.Id,
            Sku = "SKU-001",
            Name = "Widget",
            CategoryId = category.Id,
            UnitPrice = 15.00m
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("SKU-001", dto.Sku);
        Assert.Equal(15.00m, dto.UnitPrice);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenCategoryDoesNotExist()
    {
        var categoryRepository = new FakeCategoryRepository();
        var existingCategory = Category.Create("Beverages");
        categoryRepository.Seed(existingCategory);

        var productRepository = new FakeProductRepository();
        var product = Product.Create("SKU-001", "Widget", existingCategory.Id, 9.99m);
        productRepository.Seed(product);

        var handler = new UpdateProductCommandHandler(productRepository, categoryRepository);

        var command = new UpdateProductCommand
        {
            Id = product.Id,
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
        var activeCategory = Category.Create("Beverages");
        var inactiveCategory = Category.Create("Snacks");
        inactiveCategory.Deactivate();
        categoryRepository.Seed(activeCategory);
        categoryRepository.Seed(inactiveCategory);

        var productRepository = new FakeProductRepository();
        var product = Product.Create("SKU-001", "Widget", activeCategory.Id, 9.99m);
        productRepository.Seed(product);

        var handler = new UpdateProductCommandHandler(productRepository, categoryRepository);

        var command = new UpdateProductCommand
        {
            Id = product.Id,
            Sku = "SKU-001",
            Name = "Widget",
            CategoryId = inactiveCategory.Id,
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
        var product = Product.Create("SKU-001", "Widget", category.Id, 9.99m);
        productRepository.Seed(product);

        var handler = new UpdateProductCommandHandler(productRepository, categoryRepository);

        var command = new UpdateProductCommand
        {
            Id = product.Id,
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
