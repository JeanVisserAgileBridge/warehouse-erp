using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Procurement.PurchaseOrders;

public interface IPurchaseOrderRepository
{
    // Returns a tracked aggregate (including Lines) so that Domain mutations made by
    // command handlers (AddLine/RemoveLine/ReceiveProduct, etc.) are picked up by EF Core's
    // change tracker automatically. There is no corresponding UpdateAsync: callers mutate the
    // aggregate returned here and then commit through IUnitOfWork.
    Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<PurchaseOrder>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken);

    // Implementations must match order numbers case-insensitively.
    Task<PurchaseOrder?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken);

    // Does not call SaveChangesAsync: Purchase Order writes commit through the caller's
    // IUnitOfWork, consistent with the Inventory feature's transactional convention.
    Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken);
}
