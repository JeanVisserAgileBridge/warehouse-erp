using Microsoft.EntityFrameworkCore;
using WarehouseERP.Application.Procurement.PurchaseOrders;
using WarehouseERP.Domain.Procurement;
using WarehouseERP.Infrastructure.Persistence;

namespace WarehouseERP.Infrastructure.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly WarehouseErpDbContext _context;

    public PurchaseOrderRepository(WarehouseErpDbContext context)
    {
        _context = context;
    }

    // Tracked (not AsNoTracking): callers mutate the returned aggregate via its Domain methods
    // and commit through IUnitOfWork, relying on EF Core's change tracker to detect the
    // resulting scalar and Lines collection changes.
    public async Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.PurchaseOrders
            .Include(purchaseOrder => purchaseOrder.Lines)
            .FirstOrDefaultAsync(purchaseOrder => purchaseOrder.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.PurchaseOrders
            .AsNoTracking()
            .Include(purchaseOrder => purchaseOrder.Lines)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PurchaseOrder>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken)
    {
        return await _context.PurchaseOrders
            .AsNoTracking()
            .Include(purchaseOrder => purchaseOrder.Lines)
            .Where(purchaseOrder => purchaseOrder.SupplierId == supplierId)
            .ToListAsync(cancellationToken);
    }

    public async Task<PurchaseOrder?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken)
    {
        // PurchaseOrder.OrderNumber is configured with a case-insensitive collation (see PurchaseOrderConfiguration).
        return await _context.PurchaseOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(purchaseOrder => purchaseOrder.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken)
    {
        await _context.PurchaseOrders.AddAsync(purchaseOrder, cancellationToken);
    }
}
