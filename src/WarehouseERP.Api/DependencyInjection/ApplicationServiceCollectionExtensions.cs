using WarehouseERP.Application.Common;
using WarehouseERP.Application.Inventory.InventoryItems;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.AdjustStock;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.ChangeReorderLevel;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.CreateInventoryItem;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.IssueStock;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.ReceiveStock;
using WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryByProductId;
using WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryByStorageLocationId;
using WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryItemById;
using WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryItems;
using WarehouseERP.Application.Inventory.StockMovements;
using WarehouseERP.Application.Inventory.StockMovements.Queries.GetStockMovementsByInventoryItemId;
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
using WarehouseERP.Application.Procurement.PurchaseOrders;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.AddPurchaseOrderLine;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.CancelPurchaseOrder;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.CreatePurchaseOrder;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.ReceivePurchaseOrderLine;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.RemovePurchaseOrderLine;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.SubmitPurchaseOrder;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.UpdatePurchaseOrderLine;
using WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrderById;
using WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrders;
using WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrdersBySupplierId;
using WarehouseERP.Application.Procurement.Suppliers;
using WarehouseERP.Application.Procurement.Suppliers.Commands.ActivateSupplier;
using WarehouseERP.Application.Procurement.Suppliers.Commands.CreateSupplier;
using WarehouseERP.Application.Procurement.Suppliers.Commands.DeactivateSupplier;
using WarehouseERP.Application.Procurement.Suppliers.Commands.UpdateSupplier;
using WarehouseERP.Application.Procurement.Suppliers.Queries.GetSupplierById;
using WarehouseERP.Application.Procurement.Suppliers.Queries.GetSuppliers;
using WarehouseERP.Application.Reporting.Dashboard.Queries.GetDashboardSummary;
using WarehouseERP.Application.Sales.Customers;
using WarehouseERP.Application.Sales.Customers.Commands.ActivateCustomer;
using WarehouseERP.Application.Sales.Customers.Commands.CreateCustomer;
using WarehouseERP.Application.Sales.Customers.Commands.DeactivateCustomer;
using WarehouseERP.Application.Sales.Customers.Commands.UpdateCustomer;
using WarehouseERP.Application.Sales.Customers.Queries.GetCustomerById;
using WarehouseERP.Application.Sales.Customers.Queries.GetCustomers;
using WarehouseERP.Application.Sales.SalesOrders;
using WarehouseERP.Application.Sales.SalesOrders.Commands.AddSalesOrderLine;
using WarehouseERP.Application.Sales.SalesOrders.Commands.CancelSalesOrder;
using WarehouseERP.Application.Sales.SalesOrders.Commands.ConfirmSalesOrder;
using WarehouseERP.Application.Sales.SalesOrders.Commands.CreateSalesOrder;
using WarehouseERP.Application.Sales.SalesOrders.Commands.FulfilSalesOrderLine;
using WarehouseERP.Application.Sales.SalesOrders.Commands.RemoveSalesOrderLine;
using WarehouseERP.Application.Sales.SalesOrders.Commands.UpdateSalesOrderLine;
using WarehouseERP.Application.Sales.SalesOrders.Queries.GetSalesOrderById;
using WarehouseERP.Application.Sales.SalesOrders.Queries.GetSalesOrders;
using WarehouseERP.Application.Sales.SalesOrders.Queries.GetSalesOrdersByCustomerId;
using WarehouseERP.Application.Warehouses.StorageLocations;
using WarehouseERP.Application.Warehouses.StorageLocations.Commands.ActivateStorageLocation;
using WarehouseERP.Application.Warehouses.StorageLocations.Commands.CreateStorageLocation;
using WarehouseERP.Application.Warehouses.StorageLocations.Commands.DeactivateStorageLocation;
using WarehouseERP.Application.Warehouses.StorageLocations.Commands.UpdateStorageLocation;
using WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocationById;
using WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocations;
using WarehouseERP.Application.Warehouses.StorageLocations.Queries.GetStorageLocationsByWarehouseId;
using WarehouseERP.Application.Warehouses.Warehouses;
using WarehouseERP.Application.Warehouses.Warehouses.Commands.ActivateWarehouse;
using WarehouseERP.Application.Warehouses.Warehouses.Commands.CreateWarehouse;
using WarehouseERP.Application.Warehouses.Warehouses.Commands.DeactivateWarehouse;
using WarehouseERP.Application.Warehouses.Warehouses.Commands.UpdateWarehouse;
using WarehouseERP.Application.Warehouses.Warehouses.Queries.GetWarehouseById;
using WarehouseERP.Application.Warehouses.Warehouses.Queries.GetWarehouses;
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

        services.AddScoped<IQueryHandler<GetCustomersQuery, IReadOnlyList<CustomerDto>>, GetCustomersQueryHandler>();
        services.AddScoped<IQueryHandler<GetCustomerByIdQuery, CustomerDto>, GetCustomerByIdQueryHandler>();
        services.AddScoped<ICommandHandler<CreateCustomerCommand, CustomerDto>, CreateCustomerCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateCustomerCommand, CustomerDto>, UpdateCustomerCommandHandler>();
        services.AddScoped<ICommandHandler<ActivateCustomerCommand, CustomerDto>, ActivateCustomerCommandHandler>();
        services.AddScoped<ICommandHandler<DeactivateCustomerCommand, CustomerDto>, DeactivateCustomerCommandHandler>();

        services.AddScoped<IQueryHandler<GetWarehousesQuery, IReadOnlyList<WarehouseDto>>, GetWarehousesQueryHandler>();
        services.AddScoped<IQueryHandler<GetWarehouseByIdQuery, WarehouseDto>, GetWarehouseByIdQueryHandler>();
        services.AddScoped<ICommandHandler<CreateWarehouseCommand, WarehouseDto>, CreateWarehouseCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateWarehouseCommand, WarehouseDto>, UpdateWarehouseCommandHandler>();
        services.AddScoped<ICommandHandler<ActivateWarehouseCommand, WarehouseDto>, ActivateWarehouseCommandHandler>();
        services.AddScoped<ICommandHandler<DeactivateWarehouseCommand, WarehouseDto>, DeactivateWarehouseCommandHandler>();

        services.AddScoped<IQueryHandler<GetStorageLocationsQuery, IReadOnlyList<StorageLocationDto>>, GetStorageLocationsQueryHandler>();
        services.AddScoped<IQueryHandler<GetStorageLocationByIdQuery, StorageLocationDto>, GetStorageLocationByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetStorageLocationsByWarehouseIdQuery, IReadOnlyList<StorageLocationDto>>, GetStorageLocationsByWarehouseIdQueryHandler>();
        services.AddScoped<ICommandHandler<CreateStorageLocationCommand, StorageLocationDto>, CreateStorageLocationCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateStorageLocationCommand, StorageLocationDto>, UpdateStorageLocationCommandHandler>();
        services.AddScoped<ICommandHandler<ActivateStorageLocationCommand, StorageLocationDto>, ActivateStorageLocationCommandHandler>();
        services.AddScoped<ICommandHandler<DeactivateStorageLocationCommand, StorageLocationDto>, DeactivateStorageLocationCommandHandler>();

        services.AddScoped<IQueryHandler<GetDashboardSummaryQuery, ApplicationDashboardSummary>, GetDashboardSummaryQueryHandler>();

        services.AddScoped<IQueryHandler<GetInventoryItemsQuery, IReadOnlyList<InventoryItemDto>>, GetInventoryItemsQueryHandler>();
        services.AddScoped<IQueryHandler<GetInventoryItemByIdQuery, InventoryItemDto>, GetInventoryItemByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetInventoryByProductIdQuery, IReadOnlyList<InventoryItemDto>>, GetInventoryByProductIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetInventoryByStorageLocationIdQuery, IReadOnlyList<InventoryItemDto>>, GetInventoryByStorageLocationIdQueryHandler>();
        services.AddScoped<ICommandHandler<CreateInventoryItemCommand, InventoryItemDto>, CreateInventoryItemCommandHandler>();
        services.AddScoped<ICommandHandler<ReceiveStockCommand, InventoryItemDto>, ReceiveStockCommandHandler>();
        services.AddScoped<ICommandHandler<IssueStockCommand, InventoryItemDto>, IssueStockCommandHandler>();
        services.AddScoped<ICommandHandler<AdjustStockCommand, InventoryItemDto>, AdjustStockCommandHandler>();
        services.AddScoped<ICommandHandler<ChangeReorderLevelCommand, InventoryItemDto>, ChangeReorderLevelCommandHandler>();

        services.AddScoped<IQueryHandler<GetStockMovementsByInventoryItemIdQuery, IReadOnlyList<StockMovementDto>>, GetStockMovementsByInventoryItemIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetPurchaseOrdersQuery, IReadOnlyList<PurchaseOrderDto>>, GetPurchaseOrdersQueryHandler>();
        services.AddScoped<IQueryHandler<GetPurchaseOrderByIdQuery, PurchaseOrderDto>, GetPurchaseOrderByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetPurchaseOrdersBySupplierIdQuery, IReadOnlyList<PurchaseOrderDto>>, GetPurchaseOrdersBySupplierIdQueryHandler>();
        services.AddScoped<ICommandHandler<CreatePurchaseOrderCommand, PurchaseOrderDto>, CreatePurchaseOrderCommandHandler>();
        services.AddScoped<ICommandHandler<AddPurchaseOrderLineCommand, PurchaseOrderDto>, AddPurchaseOrderLineCommandHandler>();
        services.AddScoped<ICommandHandler<UpdatePurchaseOrderLineCommand, PurchaseOrderDto>, UpdatePurchaseOrderLineCommandHandler>();
        services.AddScoped<ICommandHandler<RemovePurchaseOrderLineCommand, PurchaseOrderDto>, RemovePurchaseOrderLineCommandHandler>();
        services.AddScoped<ICommandHandler<SubmitPurchaseOrderCommand, PurchaseOrderDto>, SubmitPurchaseOrderCommandHandler>();
        services.AddScoped<ICommandHandler<CancelPurchaseOrderCommand, PurchaseOrderDto>, CancelPurchaseOrderCommandHandler>();
        services.AddScoped<ICommandHandler<ReceivePurchaseOrderLineCommand, PurchaseOrderDto>, ReceivePurchaseOrderLineCommandHandler>();

        services.AddScoped<IQueryHandler<GetSalesOrdersQuery, IReadOnlyList<SalesOrderDto>>, GetSalesOrdersQueryHandler>();
        services.AddScoped<IQueryHandler<GetSalesOrderByIdQuery, SalesOrderDto>, GetSalesOrderByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetSalesOrdersByCustomerIdQuery, IReadOnlyList<SalesOrderDto>>, GetSalesOrdersByCustomerIdQueryHandler>();
        services.AddScoped<ICommandHandler<CreateSalesOrderCommand, SalesOrderDto>, CreateSalesOrderCommandHandler>();
        services.AddScoped<ICommandHandler<AddSalesOrderLineCommand, SalesOrderDto>, AddSalesOrderLineCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateSalesOrderLineCommand, SalesOrderDto>, UpdateSalesOrderLineCommandHandler>();
        services.AddScoped<ICommandHandler<RemoveSalesOrderLineCommand, SalesOrderDto>, RemoveSalesOrderLineCommandHandler>();
        services.AddScoped<ICommandHandler<ConfirmSalesOrderCommand, SalesOrderDto>, ConfirmSalesOrderCommandHandler>();
        services.AddScoped<ICommandHandler<CancelSalesOrderCommand, SalesOrderDto>, CancelSalesOrderCommandHandler>();
        services.AddScoped<ICommandHandler<FulfilSalesOrderLineCommand, SalesOrderDto>, FulfilSalesOrderLineCommandHandler>();

        return services;
    }
}
