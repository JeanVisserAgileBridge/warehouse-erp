using Microsoft.EntityFrameworkCore;
using WarehouseERP.Application.Procurement.Suppliers;
using WarehouseERP.Domain.Procurement;
using WarehouseERP.Infrastructure.Persistence;

namespace WarehouseERP.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly WarehouseErpDbContext _context;

    public SupplierRepository(WarehouseErpDbContext context)
    {
        _context = context;
    }

    public async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Suppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(supplier => supplier.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Supplier>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Suppliers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Supplier?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        // Supplier.Name is configured with a case-insensitive collation (see SupplierConfiguration).
        return await _context.Suppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(supplier => supplier.Name == name, cancellationToken);
    }

    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        await _context.Suppliers.AddAsync(supplier, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
