using WarehouseERP.Application.Sales.SalesOrders;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.SalesOrders.Fakes;

public sealed class FakeSalesOrderRepository : ISalesOrderRepository
{
    private readonly List<SalesOrder> _salesOrders = new();

    public CancellationToken? LastCancellationToken { get; private set; }
    public int AddCallCount { get; private set; }

    public void Seed(SalesOrder salesOrder)
    {
        _salesOrders.Add(salesOrder);
    }

    public Task<SalesOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_salesOrders.FirstOrDefault(salesOrder => salesOrder.Id == id));
    }

    public Task<IReadOnlyList<SalesOrder>> GetAllAsync(CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<SalesOrder>>(_salesOrders.ToList());
    }

    public Task<IReadOnlyList<SalesOrder>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<SalesOrder>>(
            _salesOrders.Where(salesOrder => salesOrder.CustomerId == customerId).ToList());
    }

    public Task<SalesOrder?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_salesOrders.FirstOrDefault(
            salesOrder => string.Equals(salesOrder.OrderNumber, orderNumber, StringComparison.OrdinalIgnoreCase)));
    }

    public Task AddAsync(SalesOrder salesOrder, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        AddCallCount++;
        _salesOrders.Add(salesOrder);
        return Task.CompletedTask;
    }
}
