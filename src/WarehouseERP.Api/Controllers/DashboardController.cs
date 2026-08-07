using Microsoft.AspNetCore.Mvc;
using WarehouseERP.Api.Contracts.Dashboard;
using WarehouseERP.Application.Common;
using WarehouseERP.Application.Reporting.Dashboard.Queries.GetDashboardSummary;
using WarehouseERP.Shared.Contracts.Dashboard;
using ApplicationDashboardSummary = WarehouseERP.Application.Reporting.Dashboard.DashboardSummary;

namespace WarehouseERP.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController : ControllerBase
{
    private readonly IQueryHandler<GetDashboardSummaryQuery, ApplicationDashboardSummary> _getDashboardSummary;

    public DashboardController(
        IQueryHandler<GetDashboardSummaryQuery, ApplicationDashboardSummary> getDashboardSummary)
    {
        _getDashboardSummary = getDashboardSummary;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardSummary>> Get(CancellationToken cancellationToken)
    {
        var summary = await _getDashboardSummary.HandleAsync(new GetDashboardSummaryQuery(), cancellationToken);

        return Ok(summary.ToContract());
    }
}
