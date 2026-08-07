using Microsoft.AspNetCore.Mvc;
using WarehouseERP.Api.Contracts.Categories;
using WarehouseERP.Application.Common;
using WarehouseERP.Application.ProductCatalog.Categories;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.ActivateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.CreateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.DeactivateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.UpdateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Queries.GetCategories;
using WarehouseERP.Application.ProductCatalog.Categories.Queries.GetCategoryById;

namespace WarehouseERP.Api.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController : ControllerBase
{
    private readonly IQueryHandler<GetCategoriesQuery, IReadOnlyList<CategoryDto>> _getCategories;
    private readonly IQueryHandler<GetCategoryByIdQuery, CategoryDto> _getCategoryById;
    private readonly ICommandHandler<CreateCategoryCommand, CategoryDto> _createCategory;
    private readonly ICommandHandler<UpdateCategoryCommand, CategoryDto> _updateCategory;
    private readonly ICommandHandler<ActivateCategoryCommand, CategoryDto> _activateCategory;
    private readonly ICommandHandler<DeactivateCategoryCommand, CategoryDto> _deactivateCategory;

    public CategoriesController(
        IQueryHandler<GetCategoriesQuery, IReadOnlyList<CategoryDto>> getCategories,
        IQueryHandler<GetCategoryByIdQuery, CategoryDto> getCategoryById,
        ICommandHandler<CreateCategoryCommand, CategoryDto> createCategory,
        ICommandHandler<UpdateCategoryCommand, CategoryDto> updateCategory,
        ICommandHandler<ActivateCategoryCommand, CategoryDto> activateCategory,
        ICommandHandler<DeactivateCategoryCommand, CategoryDto> deactivateCategory)
    {
        _getCategories = getCategories;
        _getCategoryById = getCategoryById;
        _createCategory = createCategory;
        _updateCategory = updateCategory;
        _activateCategory = activateCategory;
        _deactivateCategory = deactivateCategory;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _getCategories.HandleAsync(new GetCategoriesQuery(), cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var category = await _getCategoryById.HandleAsync(new GetCategoryByIdQuery { Id = id }, cancellationToken);

        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand
        {
            Name = request.Name,
            Description = request.Description
        };

        var category = await _createCategory.HandleAsync(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> Update(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand
        {
            Id = id,
            Name = request.Name,
            Description = request.Description
        };

        var category = await _updateCategory.HandleAsync(command, cancellationToken);

        return Ok(category);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<ActionResult<CategoryDto>> Activate(Guid id, CancellationToken cancellationToken)
    {
        var category = await _activateCategory.HandleAsync(new ActivateCategoryCommand { Id = id }, cancellationToken);

        return Ok(category);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<ActionResult<CategoryDto>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var category = await _deactivateCategory.HandleAsync(new DeactivateCategoryCommand { Id = id }, cancellationToken);

        return Ok(category);
    }
}
