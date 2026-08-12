using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseERP.Api.Contracts.Categories;
using WarehouseERP.Api.DependencyInjection;
using WarehouseERP.Application.Common;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.ActivateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.CreateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.DeactivateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.UpdateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Queries.GetCategories;
using WarehouseERP.Application.ProductCatalog.Categories.Queries.GetCategoryById;
using WarehouseERP.Shared.Contracts.Categories;
using ApplicationCategoryDto = WarehouseERP.Application.ProductCatalog.Categories.CategoryDto;

namespace WarehouseERP.Api.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController : ControllerBase
{
    private readonly IQueryHandler<GetCategoriesQuery, IReadOnlyList<ApplicationCategoryDto>> _getCategories;
    private readonly IQueryHandler<GetCategoryByIdQuery, ApplicationCategoryDto> _getCategoryById;
    private readonly ICommandHandler<CreateCategoryCommand, ApplicationCategoryDto> _createCategory;
    private readonly ICommandHandler<UpdateCategoryCommand, ApplicationCategoryDto> _updateCategory;
    private readonly ICommandHandler<ActivateCategoryCommand, ApplicationCategoryDto> _activateCategory;
    private readonly ICommandHandler<DeactivateCategoryCommand, ApplicationCategoryDto> _deactivateCategory;

    public CategoriesController(
        IQueryHandler<GetCategoriesQuery, IReadOnlyList<ApplicationCategoryDto>> getCategories,
        IQueryHandler<GetCategoryByIdQuery, ApplicationCategoryDto> getCategoryById,
        ICommandHandler<CreateCategoryCommand, ApplicationCategoryDto> createCategory,
        ICommandHandler<UpdateCategoryCommand, ApplicationCategoryDto> updateCategory,
        ICommandHandler<ActivateCategoryCommand, ApplicationCategoryDto> activateCategory,
        ICommandHandler<DeactivateCategoryCommand, ApplicationCategoryDto> deactivateCategory)
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

        return Ok(categories.ToContract());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var category = await _getCategoryById.HandleAsync(new GetCategoryByIdQuery { Id = id }, cancellationToken);

        return Ok(category.ToContract());
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<CategoryDto>> Create(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand
        {
            Name = request.Name,
            Description = request.Description
        };

        var category = await _createCategory.HandleAsync(command, cancellationToken);
        var contract = category.ToContract();

        return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<CategoryDto>> Update(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand
        {
            Id = id,
            Name = request.Name,
            Description = request.Description
        };

        var category = await _updateCategory.HandleAsync(command, cancellationToken);

        return Ok(category.ToContract());
    }

    [HttpPatch("{id:guid}/activate")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<CategoryDto>> Activate(Guid id, CancellationToken cancellationToken)
    {
        var category = await _activateCategory.HandleAsync(new ActivateCategoryCommand { Id = id }, cancellationToken);

        return Ok(category.ToContract());
    }

    [HttpPatch("{id:guid}/deactivate")]
    [Authorize(Policy = PolicyNames.AdminOnly)]
    public async Task<ActionResult<CategoryDto>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var category = await _deactivateCategory.HandleAsync(new DeactivateCategoryCommand { Id = id }, cancellationToken);

        return Ok(category.ToContract());
    }
}
