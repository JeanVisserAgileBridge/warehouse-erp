using System.Net.Http.Json;
using WarehouseERP.Blazor.Infrastructure.Http;
using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Blazor.Features.Admin.Users.Services;

public sealed class AdminUserApiClient : IAdminUserApiClient
{
    private const string BaseRoute = "api/admin/users";

    private readonly HttpClient _httpClient;

    public AdminUserApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<UserSummaryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BaseRoute, cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var users = await response.Content.ReadFromJsonAsync<IReadOnlyList<UserSummaryResponse>>(cancellationToken: cancellationToken);

        return users ?? [];
    }

    public async Task<UserSummaryResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, cancellationToken);

        return await ReadUserAsync(response, cancellationToken);
    }

    public async Task<UserSummaryResponse> AssignRolesAsync(string id, AssignRolesRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseRoute}/{id}/roles", request, cancellationToken);

        return await ReadUserAsync(response, cancellationToken);
    }

    private static async Task<UserSummaryResponse> ReadUserAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var user = await response.Content.ReadFromJsonAsync<UserSummaryResponse>(cancellationToken: cancellationToken);

        return user ?? throw new ApiException((int)response.StatusCode, "The API returned an empty response.");
    }
}
