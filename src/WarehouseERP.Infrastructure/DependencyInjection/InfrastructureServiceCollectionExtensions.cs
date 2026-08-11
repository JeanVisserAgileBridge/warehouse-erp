using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseERP.Application.Common;
using WarehouseERP.Application.Inventory.InventoryItems;
using WarehouseERP.Application.Inventory.LowStock;
using WarehouseERP.Application.Inventory.StockMovements;
using WarehouseERP.Application.ProductCatalog.Categories;
using WarehouseERP.Application.ProductCatalog.Products;
using WarehouseERP.Application.Procurement.PurchaseOrders;
using WarehouseERP.Application.Procurement.Suppliers;
using WarehouseERP.Application.Reporting.Dashboard;
using WarehouseERP.Application.Sales.Customers;
using WarehouseERP.Application.Warehouses.StorageLocations;
using WarehouseERP.Application.Warehouses.Warehouses;
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
        services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IStorageLocationRepository, StorageLocationRepository>();
        services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
        services.AddScoped<IStockMovementRepository, StockMovementRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDashboardQueryService>(_ => new DashboardQueryService(connectionString));
        services.AddScoped<ILowStockInventoryQueryService>(_ => new LowStockInventoryQueryService(connectionString));

        return services;
    }
}
