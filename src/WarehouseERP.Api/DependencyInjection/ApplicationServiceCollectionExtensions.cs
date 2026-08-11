using WarehouseERP.Application.Common;
using WarehouseERP.Application.ProductCatalog.Categories;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.ActivateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.CreateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.DeactivateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Commands.UpdateCategory;
using WarehouseERP.Application.ProductCatalog.Categories.Queries.GetCategories;
using WarehouseERP.Application.ProductCatalog.Categories.Queries.GetCategoryById;
using WarehouseERP.Application.ProductCatalog.Products;
using WarehouseERP.Application.ProductCatalog.Products.Commands.ActivateProduct;
using WarehouseERP.Application.ProductCatalog.Products.Commands.CreateProduct;
using WarehouseERP.Application.ProductCatalog.Products.Commands.DeactivateProduct;
using WarehouseERP.Application.ProductCatalog.Products.Commands.UpdateProduct;
using WarehouseERP.Application.ProductCatalog.Products.Queries.GetProductById;
using WarehouseERP.Application.ProductCatalog.Products.Queries.GetProducts;
using WarehouseERP.Application.Procurement.Suppliers;
using WarehouseERP.Application.Procurement.Suppliers.Commands.ActivateSupplier;
using WarehouseERP.Application.Procurement.Suppliers.Commands.CreateSupplier;
using WarehouseERP.Application.Procurement.Suppliers.Commands.DeactivateSupplier;
using WarehouseERP.Application.Procurement.Suppliers.Commands.UpdateSupplier;
using WarehouseERP.Application.Procurement.Suppliers.Queries.GetSupplierById;
using WarehouseERP.Application.Procurement.Suppliers.Queries.GetSuppliers;
using WarehouseERP.Application.Reporting.Dashboard.Queries.GetDashboardSummary;
using ApplicationDashboardSummary = WarehouseERP.Application.Reporting.Dashboard.DashboardSummary;

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

        services.AddScoped<IQueryHandler<GetProductsQuery, IReadOnlyList<ProductDto>>, GetProductsQueryHandler>();
        services.AddScoped<IQueryHandler<GetProductByIdQuery, ProductDto>, GetProductByIdQueryHandler>();
        services.AddScoped<ICommandHandler<CreateProductCommand, ProductDto>, CreateProductCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateProductCommand, ProductDto>, UpdateProductCommandHandler>();
        services.AddScoped<ICommandHandler<ActivateProductCommand, ProductDto>, ActivateProductCommandHandler>();
        services.AddScoped<ICommandHandler<DeactivateProductCommand, ProductDto>, DeactivateProductCommandHandler>();

        services.AddScoped<IQueryHandler<GetSuppliersQuery, IReadOnlyList<SupplierDto>>, GetSuppliersQueryHandler>();
        services.AddScoped<IQueryHandler<GetSupplierByIdQuery, SupplierDto>, GetSupplierByIdQueryHandler>();
        services.AddScoped<ICommandHandler<CreateSupplierCommand, SupplierDto>, CreateSupplierCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateSupplierCommand, SupplierDto>, UpdateSupplierCommandHandler>();
        services.AddScoped<ICommandHandler<ActivateSupplierCommand, SupplierDto>, ActivateSupplierCommandHandler>();
        services.AddScoped<ICommandHandler<DeactivateSupplierCommand, SupplierDto>, DeactivateSupplierCommandHandler>();

        services.AddScoped<IQueryHandler<GetDashboardSummaryQuery, ApplicationDashboardSummary>, GetDashboardSummaryQueryHandler>();

        return services;
    }
}
