using Microsoft.EntityFrameworkCore;
using WarehouseERP.Application.Inventory.InventoryItems;
using WarehouseERP.Domain.Inventory;
using WarehouseERP.Infrastructure.Persistence;

namespace WarehouseERP.Infrastructure.Repositories;

public class InventoryItemRepository : IInventoryItemRepository
{
    private readonly WarehouseErpDbContext _context;

    public InventoryItemRepository(WarehouseErpDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.InventoryItems
            .AsNoTracking()
            .FirstOrDefaultAsync(inventoryItem => inventoryItem.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<InventoryItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.InventoryItems
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<InventoryItem>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        return await _context.InventoryItems
            .AsNoTracking()
            .Where(inventoryItem => inventoryItem.ProductId == productId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<InventoryItem>> GetByStorageLocationIdAsync(Guid storageLocationId, CancellationToken cancellationToken)
    {
        return await _context.InventoryItems
            .AsNoTracking()
            .Where(inventoryItem => inventoryItem.StorageLocationId == storageLocationId)
            .ToListAsync(cancellationToken);
    }

    public async Task<InventoryItem?> GetByProductIdAndStorageLocationIdAsync(Guid productId, Guid storageLocationId, CancellationToken cancellationToken)
    {
        return await _context.InventoryItems
            .AsNoTracking()
            .FirstOrDefaultAsync(
                inventoryItem => inventoryItem.ProductId == productId && inventoryItem.StorageLocationId == storageLocationId,
                cancellationToken);
    }

    // Does not call SaveChangesAsync: ReceiveStock/IssueStock/AdjustStock must commit this
    // together with a StockMovement, so the caller's IUnitOfWork owns the commit point.
    public async Task AddAsync(InventoryItem inventoryItem, CancellationToken cancellationToken)
    {
        await _context.InventoryItems.AddAsync(inventoryItem, cancellationToken);
    }

    public Task UpdateAsync(InventoryItem inventoryItem, CancellationToken cancellationToken)
    {
        _context.InventoryItems.Update(inventoryItem);
        return Task.CompletedTask;
    }
}
