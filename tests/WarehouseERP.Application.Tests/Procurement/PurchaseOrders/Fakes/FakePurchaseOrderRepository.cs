using WarehouseERP.Application.Procurement.PurchaseOrders;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Fakes;

public sealed class FakePurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly List<PurchaseOrder> _purchaseOrders = new();

    public CancellationToken? LastCancellationToken { get; private set; }
    public int AddCallCount { get; private set; }

    public void Seed(PurchaseOrder purchaseOrder)
    {
        _purchaseOrders.Add(purchaseOrder);
    }

    public Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_purchaseOrders.FirstOrDefault(purchaseOrder => purchaseOrder.Id == id));
    }

    public Task<IReadOnlyList<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<PurchaseOrder>>(_purchaseOrders.ToList());
    }

    public Task<IReadOnlyList<PurchaseOrder>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<PurchaseOrder>>(
            _purchaseOrders.Where(purchaseOrder => purchaseOrder.SupplierId == supplierId).ToList());
    }

    public Task<PurchaseOrder?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_purchaseOrders.FirstOrDefault(
            purchaseOrder => string.Equals(purchaseOrder.OrderNumber, orderNumber, StringComparison.OrdinalIgnoreCase)));
    }

    public Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        AddCallCount++;
        _purchaseOrders.Add(purchaseOrder);
        return Task.CompletedTask;
    }
}
