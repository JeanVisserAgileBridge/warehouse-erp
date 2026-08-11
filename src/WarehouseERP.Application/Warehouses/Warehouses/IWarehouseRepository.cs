using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Application.Warehouses.Warehouses;

public interface IWarehouseRepository
{
    Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Warehouse>> GetAllAsync(CancellationToken cancellationToken);

    // Implementations must match codes case-insensitively.
    Task<Warehouse?> GetByCodeAsync(string code, CancellationToken cancellationToken);

    Task AddAsync(Warehouse warehouse, CancellationToken cancellationToken);

    Task UpdateAsync(Warehouse warehouse, CancellationToken cancellationToken);
}
