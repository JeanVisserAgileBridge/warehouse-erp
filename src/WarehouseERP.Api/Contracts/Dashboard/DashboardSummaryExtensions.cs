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
            InactiveProducts = summary.InactiveProducts
        };
    }
}
