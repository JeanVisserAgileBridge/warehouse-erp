using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Sales.Customers;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken);

    // Implementations must match names case-insensitively.
    Task<Customer?> GetByNameAsync(string name, CancellationToken cancellationToken);

    Task AddAsync(Customer customer, CancellationToken cancellationToken);

    Task UpdateAsync(Customer customer, CancellationToken cancellationToken);
}
