using WarehouseERP.Infrastructure.Identity;
using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Api.Contracts.Auth;

internal static class UserSummaryResponseExtensions
{
    public static UserSummaryResponse ToUserSummaryContract(this ApplicationUser user, IList<string> roles)
    {
        return new UserSummaryResponse
        {
            Id = user.Id,
            Email = user.Email ?? user.UserName ?? string.Empty,
            Roles = roles.ToArray()
        };
    }
}
