using WarehouseERP.Api.Contracts.Auth;
using WarehouseERP.Infrastructure.Identity;
using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Api.Tests.Contracts.Auth;

public class UserSummaryResponseExtensionsTests
{
    [Fact]
    public void ToContract_MapsIdEmailAndRoles()
    {
        var user = new ApplicationUser
        {
            Id = "user-1",
            Email = "purchasing.user@example.com",
            UserName = "purchasing.user@example.com"
        };

        var contract = user.ToUserSummaryContract(new[] { RoleNames.Purchasing, RoleNames.Admin });

        Assert.Equal("user-1", contract.Id);
        Assert.Equal("purchasing.user@example.com", contract.Email);
        Assert.Equal(new[] { RoleNames.Purchasing, RoleNames.Admin }, contract.Roles);
    }
}
