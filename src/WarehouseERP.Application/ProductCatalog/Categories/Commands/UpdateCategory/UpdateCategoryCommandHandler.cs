using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.ProductCatalog.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto> HandleAsync(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Category with id '{command.Id}' was not found.");

        var categoryWithSameName = await _categoryRepository.GetByNameAsync(command.Name, cancellationToken);
        if (categoryWithSameName is not null && categoryWithSameName.Id != category.Id)
        {
            throw new DuplicateNameException($"A category named '{command.Name}' already exists.");
        }

        category.Rename(command.Name);
        category.UpdateDescription(command.Description);

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return CategoryDto.FromDomain(category);
    }
}
