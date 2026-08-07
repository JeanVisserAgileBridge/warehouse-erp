using WarehouseERP.Shared.Contracts.Dashboard;

namespace WarehouseERP.Blazor.Features.Dashboard.Services;

public interface IDashboardApiClient
{
    Task<DashboardSummary> GetSummaryAsync(CancellationToken cancellationToken = default);
}
