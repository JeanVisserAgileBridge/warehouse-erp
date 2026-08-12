using WarehouseERP.Application.Reporting.Dashboard;

namespace WarehouseERP.Application.Tests.Reporting.Dashboard;

public class DashboardSummaryTests
{
    [Fact]
    public void LowStockPercentage_ComputesPercentage_OfLowStockItemsOverTotalInventoryItems()
    {
        var summary = CreateSummary(totalInventoryItems: 4, lowStockItemCount: 1);

        Assert.Equal(25m, summary.LowStockPercentage);
    }

    [Fact]
    public void LowStockPercentage_IsZero_WhenNoInventoryItemsExist()
    {
        var summary = CreateSummary(totalInventoryItems: 0, lowStockItemCount: 0);

        Assert.Equal(0m, summary.LowStockPercentage);
    }

    private static DashboardSummary CreateSummary(int totalInventoryItems, int lowStockItemCount)
    {
        return new DashboardSummary
        {
            TotalCategories = 0,
            ActiveCategories = 0,
            TotalProducts = 0,
            ActiveProducts = 0,
            InactiveProducts = 0,

            TotalInventoryItems = totalInventoryItems,
            TotalQuantityOnHand = 0,
            LowStockItemCount = lowStockItemCount,
            TotalInventoryValue = 0m,

            TotalWarehouses = 0,
            ActiveWarehouses = 0,
            TotalStorageLocations = 0,
            ActiveStorageLocations = 0,

            TotalPurchaseOrders = 0,
            DraftPurchaseOrders = 0,
            SubmittedPurchaseOrders = 0,
            PartiallyReceivedPurchaseOrders = 0,
            ReceivedPurchaseOrders = 0,
            OpenPurchaseOrderValue = 0m,

            TotalSalesOrders = 0,
            DraftSalesOrders = 0,
            ConfirmedSalesOrders = 0,
            PartiallyFulfilledSalesOrders = 0,
            FulfilledSalesOrders = 0,
            OpenSalesOrderValue = 0m
        };
    }
}
