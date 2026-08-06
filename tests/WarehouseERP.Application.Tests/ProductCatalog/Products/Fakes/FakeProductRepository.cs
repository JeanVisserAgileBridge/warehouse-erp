using WarehouseERP.Application.ProductCatalog.Products;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.Tests.ProductCatalog.Products.Fakes;

public sealed class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();

    public CancellationToken? LastCancellationToken { get; private set; }

    public void Seed(Product product)
    {
        _products.Add(product);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
    }

    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<Product>>(_products.ToList());
    }

    public Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_products.FirstOrDefault(p => string.Equals(p.Sku, sku, StringComparison.OrdinalIgnoreCase)));
    }

    public Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.CompletedTask;
    }
}
