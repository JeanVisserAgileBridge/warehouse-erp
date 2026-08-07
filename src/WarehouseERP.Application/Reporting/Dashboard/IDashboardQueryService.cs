namespace WarehouseERP.Application.Reporting.Dashboard;

public interface IDashboardQueryService
{
    Task<DashboardSummary> GetSummaryAsync(CancellationToken cancellationToken);
}
