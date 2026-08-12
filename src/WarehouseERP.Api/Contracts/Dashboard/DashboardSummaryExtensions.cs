using WarehouseERP.Shared.Contracts.Dashboard;
using ApplicationDashboardSummary = WarehouseERP.Application.Reporting.Dashboard.DashboardSummary;

namespace WarehouseERP.Api.Contracts.Dashboard;

internal static class DashboardSummaryExtensions
{
    public static DashboardSummary ToContract(this ApplicationDashboardSummary summary)
    {
        return new DashboardSummary
        {
            TotalCategories = summary.TotalCategories,
            ActiveCategories = summary.ActiveCategories,
            TotalProducts = summary.TotalProducts,
            ActiveProducts = summary.ActiveProducts,
            InactiveProducts = summary.InactiveProducts,

            TotalInventoryItems = summary.TotalInventoryItems,
            TotalQuantityOnHand = summary.TotalQuantityOnHand,
            LowStockItemCount = summary.LowStockItemCount,
            TotalInventoryValue = summary.TotalInventoryValue,

            TotalWarehouses = summary.TotalWarehouses,
            ActiveWarehouses = summary.ActiveWarehouses,
            TotalStorageLocations = summary.TotalStorageLocations,
            ActiveStorageLocations = summary.ActiveStorageLocations,

            TotalPurchaseOrders = summary.TotalPurchaseOrders,
            DraftPurchaseOrders = summary.DraftPurchaseOrders,
            SubmittedPurchaseOrders = summary.SubmittedPurchaseOrders,
            PartiallyReceivedPurchaseOrders = summary.PartiallyReceivedPurchaseOrders,
            ReceivedPurchaseOrders = summary.ReceivedPurchaseOrders,
            OpenPurchaseOrderValue = summary.OpenPurchaseOrderValue,

            TotalSalesOrders = summary.TotalSalesOrders,
            DraftSalesOrders = summary.DraftSalesOrders,
            ConfirmedSalesOrders = summary.ConfirmedSalesOrders,
            PartiallyFulfilledSalesOrders = summary.PartiallyFulfilledSalesOrders,
            FulfilledSalesOrders = summary.FulfilledSalesOrders,
            OpenSalesOrderValue = summary.OpenSalesOrderValue
        };
    }
}
