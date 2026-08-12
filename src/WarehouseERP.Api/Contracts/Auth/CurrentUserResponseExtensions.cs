using WarehouseERP.Infrastructure.Identity;
using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Api.Contracts.Auth;

internal static class CurrentUserResponseExtensions
{
    public static CurrentUserResponse ToContract(this ApplicationUser user, IList<string> roles)
    {
        return new CurrentUserResponse
        {
            Id = user.Id,
            Email = user.Email ?? user.UserName ?? string.Empty,
            Roles = roles.ToArray()
        };
    }
}
