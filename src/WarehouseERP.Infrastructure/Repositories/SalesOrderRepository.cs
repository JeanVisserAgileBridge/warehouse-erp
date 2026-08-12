using Microsoft.EntityFrameworkCore;
using WarehouseERP.Application.Sales.SalesOrders;
using WarehouseERP.Domain.Sales;
using WarehouseERP.Infrastructure.Persistence;

namespace WarehouseERP.Infrastructure.Repositories;

public class SalesOrderRepository : ISalesOrderRepository
{
    private readonly WarehouseErpDbContext _context;

    public SalesOrderRepository(WarehouseErpDbContext context)
    {
        _context = context;
    }

    // Tracked (not AsNoTracking): callers mutate the returned aggregate via its Domain methods
    // and commit through IUnitOfWork, relying on EF Core's change tracker to detect the
    // resulting scalar and Lines collection changes.
    public async Task<SalesOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.SalesOrders
            .Include(salesOrder => salesOrder.Lines)
            .FirstOrDefaultAsync(salesOrder => salesOrder.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<SalesOrder>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.SalesOrders
            .AsNoTracking()
            .Include(salesOrder => salesOrder.Lines)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SalesOrder>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        return await _context.SalesOrders
            .AsNoTracking()
            .Include(salesOrder => salesOrder.Lines)
            .Where(salesOrder => salesOrder.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<SalesOrder?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken)
    {
        // SalesOrder.OrderNumber is configured with a case-insensitive collation (see SalesOrderConfiguration).
        return await _context.SalesOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(salesOrder => salesOrder.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task AddAsync(SalesOrder salesOrder, CancellationToken cancellationToken)
    {
        await _context.SalesOrders.AddAsync(salesOrder, cancellationToken);
    }
}
