using WarehouseERP.Application.Sales.Customers;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.Customers.Fakes;

public sealed class FakeCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = new();

    public CancellationToken? LastCancellationToken { get; private set; }

    public void Seed(Customer customer)
    {
        _customers.Add(customer);
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_customers.FirstOrDefault(c => c.Id == id));
    }

    public Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<Customer>>(_customers.ToList());
    }

    public Task<Customer?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_customers.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)));
    }

    public Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        _customers.Add(customer);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Customer customer, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.CompletedTask;
    }
}
