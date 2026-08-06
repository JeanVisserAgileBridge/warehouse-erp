using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.ProductCatalog.Products;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken);

    // Implementations must match SKUs case-insensitively.
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken);

    Task AddAsync(Product product, CancellationToken cancellationToken);

    Task UpdateAsync(Product product, CancellationToken cancellationToken);
}
