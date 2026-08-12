using System.Net;
using System.Net.Http.Json;
using WarehouseERP.Blazor.Infrastructure.Http;
using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Blazor.Features.Auth.Services;

public sealed class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login?useCookies=true", request, cancellationToken);

        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);
    }

    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/logout", new { }, cancellationToken);

        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);
    }

    public async Task<CurrentUserResponse?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("api/users/me", cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return null;
        }

        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        return await response.Content.ReadFromJsonAsync<CurrentUserResponse>(cancellationToken: cancellationToken);
    }
}
