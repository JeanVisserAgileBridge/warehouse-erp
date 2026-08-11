using WarehouseERP.Application.Procurement.Suppliers;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.Suppliers.Fakes;

public sealed class FakeSupplierRepository : ISupplierRepository
{
    private readonly List<Supplier> _suppliers = new();

    public CancellationToken? LastCancellationToken { get; private set; }

    public void Seed(Supplier supplier)
    {
        _suppliers.Add(supplier);
    }

    public Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_suppliers.FirstOrDefault(s => s.Id == id));
    }

    public Task<IReadOnlyList<Supplier>> GetAllAsync(CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<Supplier>>(_suppliers.ToList());
    }

    public Task<Supplier?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_suppliers.FirstOrDefault(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase)));
    }

    public Task AddAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        _suppliers.Add(supplier);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.CompletedTask;
    }
}
