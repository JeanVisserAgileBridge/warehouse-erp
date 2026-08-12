using WarehouseERP.Application.Reporting.Dashboard;

namespace WarehouseERP.Application.Tests.Reporting.Dashboard.Fakes;

public sealed class FakeDashboardQueryService : IDashboardQueryService
{
    private readonly DashboardSummary _summary;

    public CancellationToken? LastCancellationToken { get; private set; }

    public FakeDashboardQueryService(DashboardSummary summary)
    {
        _summary = summary;
    }

    public Task<DashboardSummary> GetSummaryAsync(CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_summary);
    }
}
