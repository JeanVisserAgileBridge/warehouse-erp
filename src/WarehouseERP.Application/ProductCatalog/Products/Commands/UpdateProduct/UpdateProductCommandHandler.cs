using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.ProductCatalog.Categories;

namespace WarehouseERP.Application.ProductCatalog.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductDto> HandleAsync(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Product with id '{command.Id}' was not found.");

        var productWithSameSku = await _productRepository.GetBySkuAsync(command.Sku, cancellationToken);
        if (productWithSameSku is not null && productWithSameSku.Id != product.Id)
        {
            throw new DuplicateSkuException($"A product with SKU '{command.Sku}' already exists.");
        }

        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken)
            ?? throw new NotFoundException($"Category with id '{command.CategoryId}' was not found.");

        if (!category.IsActive)
        {
            throw new InactiveCategoryException($"Category with id '{command.CategoryId}' is not active.");
        }

        product.Rename(command.Name);
        product.ChangeSku(command.Sku);
        product.ChangeDescription(command.Description);
        product.ChangePrice(command.UnitPrice);
        product.ChangeCategory(command.CategoryId);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return ProductDto.FromDomain(product);
    }
}
