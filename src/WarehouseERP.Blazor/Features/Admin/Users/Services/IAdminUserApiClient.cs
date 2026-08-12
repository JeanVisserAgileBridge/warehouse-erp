using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Blazor.Features.Admin.Users.Services;

public interface IAdminUserApiClient
{
    Task<IReadOnlyList<UserSummaryResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<UserSummaryResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);

    Task<UserSummaryResponse> AssignRolesAsync(string id, AssignRolesRequest request, CancellationToken cancellationToken = default);
}
