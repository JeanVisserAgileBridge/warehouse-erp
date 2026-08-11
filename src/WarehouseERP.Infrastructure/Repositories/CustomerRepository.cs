using Microsoft.EntityFrameworkCore;
using WarehouseERP.Application.Sales.Customers;
using WarehouseERP.Domain.Sales;
using WarehouseERP.Infrastructure.Persistence;

namespace WarehouseERP.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly WarehouseErpDbContext _context;

    public CustomerRepository(WarehouseErpDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Customers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Customer?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        // Customer.Name is configured with a case-insensitive collation (see CustomerConfiguration).
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.Name == name, cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
