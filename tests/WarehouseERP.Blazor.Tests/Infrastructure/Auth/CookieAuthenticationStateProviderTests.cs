using Microsoft.AspNetCore.Components.Authorization;
using WarehouseERP.Blazor.Features.Auth.Services;
using WarehouseERP.Blazor.Infrastructure.Auth;
using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Blazor.Tests.Infrastructure.Auth;

public class CookieAuthenticationStateProviderTests
{
    [Fact]
    public async Task GetAuthenticationStateAsync_AddsRoleClaimsFromCurrentUser()
    {
        var currentUser = new CurrentUserResponse
        {
            Id = "user-1",
            Email = "warehouse.user@example.com",
            Roles = new[] { RoleNames.Warehouse }
        };

        var provider = new CookieAuthenticationStateProvider(new StubAuthApiClient(currentUser));

        var state = await provider.GetAuthenticationStateAsync();

        Assert.True(state.User.IsInRole(RoleNames.Warehouse));
        Assert.False(state.User.IsInRole(RoleNames.Admin));
        Assert.Equal("warehouse.user@example.com", state.User.Identity?.Name);
    }

    [Fact]
    public async Task GetAuthenticationStateAsync_ReturnsAnonymousWhenNoCurrentUser()
    {
        var provider = new CookieAuthenticationStateProvider(new StubAuthApiClient(null));

        var state = await provider.GetAuthenticationStateAsync();

        Assert.False(state.User.Identity?.IsAuthenticated ?? false);
    }

    [Fact]
    public void NotifyUserAuthenticated_RaisesStateWithRoleClaims()
    {
        var provider = new CookieAuthenticationStateProvider(new StubAuthApiClient(null));
        AuthenticationState? notifiedState = null;
        provider.AuthenticationStateChanged += task => notifiedState = task.Result;

        provider.NotifyUserAuthenticated(new CurrentUserResponse
        {
            Id = "user-2",
            Email = "sales.user@example.com",
            Roles = new[] { RoleNames.Sales }
        });

        Assert.NotNull(notifiedState);
        Assert.True(notifiedState!.User.IsInRole(RoleNames.Sales));
    }

    private sealed class StubAuthApiClient : IAuthApiClient
    {
        private readonly CurrentUserResponse? _currentUser;

        public StubAuthApiClient(CurrentUserResponse? currentUser)
        {
            _currentUser = currentUser;
        }

        public Task LoginAsync(LoginRequest request, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LogoutAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task<CurrentUserResponse?> GetCurrentUserAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(_currentUser);
    }
}
