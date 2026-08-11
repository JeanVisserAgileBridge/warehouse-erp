using Microsoft.EntityFrameworkCore;
using WarehouseERP.Application.Warehouses.StorageLocations;
using WarehouseERP.Domain.Warehouses;
using WarehouseERP.Infrastructure.Persistence;

namespace WarehouseERP.Infrastructure.Repositories;

public class StorageLocationRepository : IStorageLocationRepository
{
    private readonly WarehouseErpDbContext _context;

    public StorageLocationRepository(WarehouseErpDbContext context)
    {
        _context = context;
    }

    public async Task<StorageLocation?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.StorageLocations
            .AsNoTracking()
            .FirstOrDefaultAsync(storageLocation => storageLocation.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<StorageLocation>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.StorageLocations
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StorageLocation>> GetByWarehouseIdAsync(Guid warehouseId, CancellationToken cancellationToken)
    {
        return await _context.StorageLocations
            .AsNoTracking()
            .Where(storageLocation => storageLocation.WarehouseId == warehouseId)
            .ToListAsync(cancellationToken);
    }

    public async Task<StorageLocation?> GetByWarehouseIdAndCodeAsync(Guid warehouseId, string code, CancellationToken cancellationToken)
    {
        // StorageLocation.Code is configured with a case-insensitive collation (see StorageLocationConfiguration).
        return await _context.StorageLocations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                storageLocation => storageLocation.WarehouseId == warehouseId && storageLocation.Code == code,
                cancellationToken);
    }

    public async Task AddAsync(StorageLocation storageLocation, CancellationToken cancellationToken)
    {
        await _context.StorageLocations.AddAsync(storageLocation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(StorageLocation storageLocation, CancellationToken cancellationToken)
    {
        _context.StorageLocations.Update(storageLocation);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
