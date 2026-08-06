using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.ProductCatalog.Products.Commands.ActivateProduct;

public sealed class ActivateProductCommandHandler : ICommandHandler<ActivateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public ActivateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> HandleAsync(ActivateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Product with id '{command.Id}' was not found.");

        product.Activate();

        await _productRepository.UpdateAsync(product, cancellationToken);

        return ProductDto.FromDomain(product);
    }
}
