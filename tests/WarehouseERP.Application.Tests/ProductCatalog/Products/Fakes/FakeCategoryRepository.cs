using WarehouseERP.Application.ProductCatalog.Categories;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.Tests.ProductCatalog.Products.Fakes;

public sealed class FakeCategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories = new();

    public CancellationToken? LastCancellationToken { get; private set; }

    public void Seed(Category category)
    {
        _categories.Add(category);
    }

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_categories.FirstOrDefault(c => c.Id == id));
    }

    public Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<Category>>(_categories.ToList());
    }

    public Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_categories.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)));
    }

    public Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        _categories.Add(category);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.CompletedTask;
    }
}
