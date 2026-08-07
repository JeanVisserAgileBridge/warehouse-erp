using System.Net.Http.Json;
using WarehouseERP.Blazor.Infrastructure.Http;
using WarehouseERP.Shared.Contracts.Dashboard;

namespace WarehouseERP.Blazor.Features.Dashboard.Services;

public sealed class DashboardApiClient : IDashboardApiClient
{
    private const string BaseRoute = "api/dashboard";

    private readonly HttpClient _httpClient;

    public DashboardApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DashboardSummary> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BaseRoute, cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var summary = await response.Content.ReadFromJsonAsync<DashboardSummary>(cancellationToken: cancellationToken);

        return summary ?? throw new ApiException((int)response.StatusCode, "The API returned an empty response.");
    }
}
