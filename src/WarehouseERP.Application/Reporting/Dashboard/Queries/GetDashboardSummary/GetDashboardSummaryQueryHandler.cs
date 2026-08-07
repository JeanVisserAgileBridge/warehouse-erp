using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Reporting.Dashboard.Queries.GetDashboardSummary;

public sealed class GetDashboardSummaryQueryHandler : IQueryHandler<GetDashboardSummaryQuery, DashboardSummary>
{
    private readonly IDashboardQueryService _dashboardQueryService;

    public GetDashboardSummaryQueryHandler(IDashboardQueryService dashboardQueryService)
    {
        _dashboardQueryService = dashboardQueryService;
    }

    public Task<DashboardSummary> HandleAsync(GetDashboardSummaryQuery query, CancellationToken cancellationToken)
    {
        return _dashboardQueryService.GetSummaryAsync(cancellationToken);
    }
}
