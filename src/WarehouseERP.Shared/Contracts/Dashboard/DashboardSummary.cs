namespace WarehouseERP.Shared.Contracts.Dashboard;

public sealed class DashboardSummary
{
    public required int TotalCategories { get; init; }
    public required int ActiveCategories { get; init; }
    public required int TotalProducts { get; init; }
    public required int ActiveProducts { get; init; }
    public required int InactiveProducts { get; init; }

    public required int TotalInventoryItems { get; init; }
    public required int TotalQuantityOnHand { get; init; }
    public required int LowStockItemCount { get; init; }
    public required decimal TotalInventoryValue { get; init; }

    public required int TotalWarehouses { get; init; }
    public required int ActiveWarehouses { get; init; }
    public required int TotalStorageLocations { get; init; }
    public required int ActiveStorageLocations { get; init; }

    public required int TotalPurchaseOrders { get; init; }
    public required int DraftPurchaseOrders { get; init; }
    public required int SubmittedPurchaseOrders { get; init; }
    public required int PartiallyReceivedPurchaseOrders { get; init; }
    public required int ReceivedPurchaseOrders { get; init; }
    public required decimal OpenPurchaseOrderValue { get; init; }

    public required int TotalSalesOrders { get; init; }
    public required int DraftSalesOrders { get; init; }
    public required int ConfirmedSalesOrders { get; init; }
    public required int PartiallyFulfilledSalesOrders { get; init; }
    public required int FulfilledSalesOrders { get; init; }
    public required decimal OpenSalesOrderValue { get; init; }

    public decimal LowStockPercentage =>
        TotalInventoryItems == 0 ? 0m : (decimal)LowStockItemCount / TotalInventoryItems * 100m;
}
