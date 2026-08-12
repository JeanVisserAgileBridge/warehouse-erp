using Dapper;
using Microsoft.Data.SqlClient;
using WarehouseERP.Application.Reporting.Dashboard;

namespace WarehouseERP.Infrastructure.Reporting;

public class DashboardQueryService : IDashboardQueryService
{
    // The 'Draft'/'Submitted'/... string literals below must stay in sync with the
    // PurchaseOrderStatus and SalesOrderStatus enum member names, since PurchaseOrderConfiguration
    // and SalesOrderConfiguration persist those enums via HasConversion<string>().
    private const string SummarySql = """
        SELECT
            (SELECT COUNT(*) FROM Categories) AS TotalCategories,
            (SELECT COUNT(*) FROM Categories WHERE IsActive = 1) AS ActiveCategories,
            (SELECT COUNT(*) FROM Products) AS TotalProducts,
            (SELECT COUNT(*) FROM Products WHERE IsActive = 1) AS ActiveProducts,
            (SELECT COUNT(*) FROM Products WHERE IsActive = 0) AS InactiveProducts,

            (SELECT COUNT(*) FROM InventoryItems) AS TotalInventoryItems,
            (SELECT COALESCE(SUM(QuantityOnHand), 0) FROM InventoryItems) AS TotalQuantityOnHand,
            (SELECT COUNT(*) FROM InventoryItems WHERE QuantityOnHand <= ReorderLevel) AS LowStockItemCount,
            (SELECT COALESCE(SUM(i.QuantityOnHand * p.UnitPrice), 0)
                FROM InventoryItems i
                INNER JOIN Products p ON p.Id = i.ProductId) AS TotalInventoryValue,

            (SELECT COUNT(*) FROM Warehouses) AS TotalWarehouses,
            (SELECT COUNT(*) FROM Warehouses WHERE IsActive = 1) AS ActiveWarehouses,
            (SELECT COUNT(*) FROM StorageLocations) AS TotalStorageLocations,
            (SELECT COUNT(*) FROM StorageLocations WHERE IsActive = 1) AS ActiveStorageLocations,

            (SELECT COUNT(*) FROM PurchaseOrders) AS TotalPurchaseOrders,
            (SELECT COUNT(*) FROM PurchaseOrders WHERE Status = 'Draft') AS DraftPurchaseOrders,
            (SELECT COUNT(*) FROM PurchaseOrders WHERE Status = 'Submitted') AS SubmittedPurchaseOrders,
            (SELECT COUNT(*) FROM PurchaseOrders WHERE Status = 'PartiallyReceived') AS PartiallyReceivedPurchaseOrders,
            (SELECT COUNT(*) FROM PurchaseOrders WHERE Status = 'Received') AS ReceivedPurchaseOrders,
            (SELECT COALESCE(SUM((pol.QuantityOrdered - pol.QuantityReceived) * pol.UnitPrice), 0)
                FROM PurchaseOrderLines pol
                INNER JOIN PurchaseOrders po ON po.Id = pol.PurchaseOrderId
                WHERE po.Status NOT IN ('Received', 'Cancelled')) AS OpenPurchaseOrderValue,

            (SELECT COUNT(*) FROM SalesOrders) AS TotalSalesOrders,
            (SELECT COUNT(*) FROM SalesOrders WHERE Status = 'Draft') AS DraftSalesOrders,
            (SELECT COUNT(*) FROM SalesOrders WHERE Status = 'Confirmed') AS ConfirmedSalesOrders,
            (SELECT COUNT(*) FROM SalesOrders WHERE Status = 'PartiallyFulfilled') AS PartiallyFulfilledSalesOrders,
            (SELECT COUNT(*) FROM SalesOrders WHERE Status = 'Fulfilled') AS FulfilledSalesOrders,
            (SELECT COALESCE(SUM((sol.QuantityOrdered - sol.QuantityFulfilled) * sol.UnitPrice), 0)
                FROM SalesOrderLines sol
                INNER JOIN SalesOrders so ON so.Id = sol.SalesOrderId
                WHERE so.Status NOT IN ('Fulfilled', 'Cancelled')) AS OpenSalesOrderValue;
        """;

    private readonly string _connectionString;

    public DashboardQueryService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<DashboardSummary> GetSummaryAsync(CancellationToken cancellationToken)
    {
        using var connection = new SqlConnection(_connectionString);

        var command = new CommandDefinition(SummarySql, cancellationToken: cancellationToken);

        return await connection.QuerySingleAsync<DashboardSummary>(command);
    }
}
