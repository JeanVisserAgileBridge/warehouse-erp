using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Blazor.Features.Auth.Services;

public interface IAuthApiClient
{
    Task LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    Task LogoutAsync(CancellationToken cancellationToken = default);

    Task<CurrentUserResponse?> GetCurrentUserAsync(CancellationToken cancellationToken = default);
}
