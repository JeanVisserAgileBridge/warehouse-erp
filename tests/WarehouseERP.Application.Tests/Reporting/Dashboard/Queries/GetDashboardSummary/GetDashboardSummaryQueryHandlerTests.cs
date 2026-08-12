using WarehouseERP.Application.Reporting.Dashboard;
using WarehouseERP.Application.Reporting.Dashboard.Queries.GetDashboardSummary;
using WarehouseERP.Application.Tests.Reporting.Dashboard.Fakes;

namespace WarehouseERP.Application.Tests.Reporting.Dashboard.Queries.GetDashboardSummary;

public class GetDashboardSummaryQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsSummary_FromDashboardQueryService()
    {
        var summary = CreateSummary();
        var dashboardQueryService = new FakeDashboardQueryService(summary);
        var handler = new GetDashboardSummaryQueryHandler(dashboardQueryService);

        var result = await handler.HandleAsync(new GetDashboardSummaryQuery(), CancellationToken.None);

        Assert.Same(summary, result);
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToDashboardQueryService()
    {
        var dashboardQueryService = new FakeDashboardQueryService(CreateSummary());
        var handler = new GetDashboardSummaryQueryHandler(dashboardQueryService);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new GetDashboardSummaryQuery(), cts.Token);

        Assert.Equal(cts.Token, dashboardQueryService.LastCancellationToken);
    }

    private static DashboardSummary CreateSummary()
    {
        return new DashboardSummary
        {
            TotalCategories = 1,
            ActiveCategories = 1,
            TotalProducts = 1,
            ActiveProducts = 1,
            InactiveProducts = 0,

            TotalInventoryItems = 1,
            TotalQuantityOnHand = 10,
            LowStockItemCount = 0,
            TotalInventoryValue = 100m,

            TotalWarehouses = 1,
            ActiveWarehouses = 1,
            TotalStorageLocations = 1,
            ActiveStorageLocations = 1,

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
