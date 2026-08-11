using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseERP.Application.Inventory.LowStock;
using WarehouseERP.Application.ProductCatalog.Categories;
using WarehouseERP.Application.ProductCatalog.Products;
using WarehouseERP.Application.Procurement.Suppliers;
using WarehouseERP.Application.Reporting.Dashboard;
using WarehouseERP.Infrastructure.Persistence;
using WarehouseERP.Infrastructure.Repositories;
using WarehouseERP.Infrastructure.Reporting;

namespace WarehouseERP.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(InfrastructureConstants.ConnectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string '{InfrastructureConstants.ConnectionStringName}' was not found.");

        services.AddDbContext<WarehouseErpDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IDashboardQueryService>(_ => new DashboardQueryService(connectionString));
        services.AddScoped<ILowStockInventoryQueryService>(_ => new LowStockInventoryQueryService(connectionString));

        return services;
    }
}
