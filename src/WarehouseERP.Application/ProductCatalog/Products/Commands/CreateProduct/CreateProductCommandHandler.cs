using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.ProductCatalog.Categories;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.ProductCatalog.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductDto> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var existingProduct = await _productRepository.GetBySkuAsync(command.Sku, cancellationToken);
        if (existingProduct is not null)
        {
            throw new DuplicateSkuException($"A product with SKU '{command.Sku}' already exists.");
        }

        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken)
            ?? throw new NotFoundException($"Category with id '{command.CategoryId}' was not found.");

        if (!category.IsActive)
        {
            throw new InactiveCategoryException($"Category with id '{command.CategoryId}' is not active.");
        }

        var product = Product.Create(command.Sku, command.Name, command.CategoryId, command.UnitPrice, command.Description);

        await _productRepository.AddAsync(product, cancellationToken);

        return ProductDto.FromDomain(product);
    }
}
