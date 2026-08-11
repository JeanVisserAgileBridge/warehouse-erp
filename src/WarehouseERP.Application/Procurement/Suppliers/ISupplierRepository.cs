using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Procurement.Suppliers;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Supplier>> GetAllAsync(CancellationToken cancellationToken);

    // Implementations must match names case-insensitively.
    Task<Supplier?> GetByNameAsync(string name, CancellationToken cancellationToken);

    Task AddAsync(Supplier supplier, CancellationToken cancellationToken);

    Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken);
}
