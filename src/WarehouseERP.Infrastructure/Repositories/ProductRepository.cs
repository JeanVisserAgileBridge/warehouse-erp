using Microsoft.EntityFrameworkCore;
using WarehouseERP.Application.ProductCatalog.Products;
using WarehouseERP.Domain.ProductCatalog;
using WarehouseERP.Infrastructure.Persistence;

namespace WarehouseERP.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly WarehouseErpDbContext _context;

    public ProductRepository(WarehouseErpDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken)
    {
        // Product.Sku is configured with a case-insensitive collation (see ProductConfiguration).
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Sku == sku, cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
