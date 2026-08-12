using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using WarehouseERP.Blazor.Features.Auth.Services;
using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Blazor.Infrastructure.Auth;

public sealed class CookieAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

    private readonly IAuthApiClient _authApiClient;

    public CookieAuthenticationStateProvider(IAuthApiClient authApiClient)
    {
        _authApiClient = authApiClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var currentUser = await _authApiClient.GetCurrentUserAsync();
        var principal = currentUser is null ? Anonymous : BuildPrincipal(currentUser);

        return new AuthenticationState(principal);
    }

    public void NotifyUserAuthenticated(CurrentUserResponse currentUser)
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(BuildPrincipal(currentUser))));
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(Anonymous)));
    }

    private static ClaimsPrincipal BuildPrincipal(CurrentUserResponse currentUser)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, currentUser.Email),
            new(ClaimTypes.Email, currentUser.Email)
        };

        claims.AddRange(currentUser.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, authenticationType: "Cookies");

        return new ClaimsPrincipal(identity);
    }
}
