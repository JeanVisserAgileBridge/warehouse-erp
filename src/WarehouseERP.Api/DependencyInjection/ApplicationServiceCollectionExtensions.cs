using WarehouseERP.Application.Common;
using WarehouseERP.Application.ProductCatalog.Categories;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.ActivateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.CreateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.DeactivateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.UpdateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Queries.GetCategories;
using WarehouseERP.Application.ProductCatalog.Categories.Queries.GetCategoryById;

namespace WarehouseERP.Api.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetCategoriesQuery, IReadOnlyList<CategoryDto>>, GetCategoriesQueryHandler>();
        services.AddScoped<IQueryHandler<GetCategoryByIdQuery, CategoryDto>, GetCategoryByIdQueryHandler>();
        services.AddScoped<ICommandHandler<CreateCategoryCommand, CategoryDto>, CreateCategoryCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateCategoryCommand, CategoryDto>, UpdateCategoryCommandHandler>();
        services.AddScoped<ICommandHandler<ActivateCategoryCommand, CategoryDto>, ActivateCategoryCommandHandler>();
        services.AddScoped<ICommandHandler<DeactivateCategoryCommand, CategoryDto>, DeactivateCategoryCommandHandler>();

        return services;
    }
}
