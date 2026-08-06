using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.ProductCatalog.Categories.Commands.ActivateCategory;

public sealed class ActivateCategoryCommandHandler : ICommandHandler<ActivateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public ActivateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto> HandleAsync(ActivateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Category with id '{command.Id}' was not found.");

        category.Activate();

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return CategoryDto.FromDomain(category);
    }
}
