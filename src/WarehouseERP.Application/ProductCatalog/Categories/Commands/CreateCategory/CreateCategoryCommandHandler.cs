using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Application.ProductCatalog.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto> HandleAsync(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var existingCategory = await _categoryRepository.GetByNameAsync(command.Name, cancellationToken);
        if (existingCategory is not null)
        {
            throw new DuplicateNameException($"A category named '{command.Name}' already exists.");
        }

        var category = Category.Create(command.Name, command.Description);

        await _categoryRepository.AddAsync(category, cancellationToken);

        return CategoryDto.FromDomain(category);
    }
}
