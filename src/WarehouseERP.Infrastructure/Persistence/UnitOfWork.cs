using WarehouseERP.Application.Common;

namespace WarehouseERP.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly WarehouseErpDbContext _context;

    public UnitOfWork(WarehouseErpDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
