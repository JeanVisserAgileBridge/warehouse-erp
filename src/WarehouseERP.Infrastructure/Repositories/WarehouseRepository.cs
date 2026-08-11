using Microsoft.EntityFrameworkCore;
using WarehouseERP.Application.Warehouses.Warehouses;
using WarehouseERP.Domain.Warehouses;
using WarehouseERP.Infrastructure.Persistence;

namespace WarehouseERP.Infrastructure.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly WarehouseErpDbContext _context;

    public WarehouseRepository(WarehouseErpDbContext context)
    {
        _context = context;
    }

    public async Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(warehouse => warehouse.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Warehouse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Warehouses
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Warehouse?> GetByCodeAsync(string code, CancellationToken cancellationToken)
    {
        // Warehouse.Code is configured with a case-insensitive collation (see WarehouseConfiguration).
        return await _context.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(warehouse => warehouse.Code == code, cancellationToken);
    }

    public async Task AddAsync(Warehouse warehouse, CancellationToken cancellationToken)
    {
        await _context.Warehouses.AddAsync(warehouse, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Warehouse warehouse, CancellationToken cancellationToken)
    {
        _context.Warehouses.Update(warehouse);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
