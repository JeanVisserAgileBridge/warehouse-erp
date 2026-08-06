using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.ProductCatalog.Products.Commands.DeactivateProduct;

public sealed class DeactivateProductCommandHandler : ICommandHandler<DeactivateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public DeactivateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> HandleAsync(DeactivateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Product with id '{command.Id}' was not found.");

        product.Deactivate();

        await _productRepository.UpdateAsync(product, cancellationToken);

        return ProductDto.FromDomain(product);
    }
}
