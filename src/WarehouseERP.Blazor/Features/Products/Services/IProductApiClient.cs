using WarehouseERP.Shared.Contracts.Products;

namespace WarehouseERP.Blazor.Features.Products.Services;

public interface IProductApiClient
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ProductDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);

    Task<ProductDto> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default);

    Task<ProductDto> ActivateAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ProductDto> DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
}
