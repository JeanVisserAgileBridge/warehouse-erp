using Microsoft.EntityFrameworkCore;
using WarehouseERP.Application.ProductCatalog.Categories;
using WarehouseERP.Domain.ProductCatalog;
using WarehouseERP.Infrastructure.Persistence;

namespace WarehouseERP.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly WarehouseErpDbContext _context;

    public CategoryRepository(WarehouseErpDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        // Category.Name is configured with a case-insensitive collation (see CategoryConfiguration).
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Name == name, cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
