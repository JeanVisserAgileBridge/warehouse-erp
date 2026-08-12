using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Sales.SalesOrders;

public interface ISalesOrderRepository
{
    // Returns a tracked aggregate (including Lines) so that Domain mutations made by
    // command handlers (AddLine/RemoveLine/FulfillProduct, etc.) are picked up by EF Core's
    // change tracker automatically. There is no corresponding UpdateAsync: callers mutate the
    // aggregate returned here and then commit through IUnitOfWork.
    Task<SalesOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<SalesOrder>> GetAllAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<SalesOrder>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);

    // Implementations must match order numbers case-insensitively.
    Task<SalesOrder?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken);

    // Does not call SaveChangesAsync: Sales Order writes commit through the caller's
    // IUnitOfWork, consistent with the Inventory and Purchase Order features' transactional convention.
    Task AddAsync(SalesOrder salesOrder, CancellationToken cancellationToken);
}
