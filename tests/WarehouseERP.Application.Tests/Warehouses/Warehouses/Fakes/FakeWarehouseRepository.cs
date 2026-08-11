using WarehouseERP.Application.Warehouses.Warehouses;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Tests.Warehouses.Warehouses.Fakes;

public sealed class FakeWarehouseRepository : IWarehouseRepository
{
    private readonly List<Warehouse> _warehouses = new();

    public CancellationToken? LastCancellationToken { get; private set; }

    public void Seed(Warehouse warehouse)
    {
        _warehouses.Add(warehouse);
    }

    public Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_warehouses.FirstOrDefault(w => w.Id == id));
    }

    public Task<IReadOnlyList<Warehouse>> GetAllAsync(CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<Warehouse>>(_warehouses.ToList());
    }

    public Task<Warehouse?> GetByCodeAsync(string code, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_warehouses.FirstOrDefault(w => string.Equals(w.Code, code, StringComparison.OrdinalIgnoreCase)));
    }

    public Task AddAsync(Warehouse warehouse, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        _warehouses.Add(warehouse);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Warehouse warehouse, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.CompletedTask;
    }
}
