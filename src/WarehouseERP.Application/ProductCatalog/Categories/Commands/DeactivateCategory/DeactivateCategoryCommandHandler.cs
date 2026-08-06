using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.ProductCatalog.Categories.Commands.DeactivateCategory;

public sealed class DeactivateCategoryCommandHandler : ICommandHandler<DeactivateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeactivateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto> HandleAsync(DeactivateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Category with id '{command.Id}' was not found.");

        category.Deactivate();

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return CategoryDto.FromDomain(category);
    }
}
