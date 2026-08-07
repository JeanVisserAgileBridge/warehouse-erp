using WarehouseERP.Shared.Contracts.Categories;

namespace WarehouseERP.Blazor.Features.Categories.Services;

public interface ICategoryApiClient
{
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CategoryDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default);

    Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken = default);

    Task<CategoryDto> ActivateAsync(Guid id, CancellationToken cancellationToken = default);

    Task<CategoryDto> DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
}
