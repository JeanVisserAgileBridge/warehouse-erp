using WarehouseERP.Api.Contracts.Auth;
using WarehouseERP.Infrastructure.Identity;
using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Api.Tests.Contracts.Auth;

public class CurrentUserResponseExtensionsTests
{
    [Fact]
    public void ToContract_MapsIdEmailAndRoles()
    {
        var user = new ApplicationUser
        {
            Id = "user-1",
            Email = "warehouse.user@example.com",
            UserName = "warehouse.user@example.com"
        };

        var contract = user.ToContract(new[] { RoleNames.Warehouse });

        Assert.Equal("user-1", contract.Id);
        Assert.Equal("warehouse.user@example.com", contract.Email);
        Assert.Equal(new[] { RoleNames.Warehouse }, contract.Roles);
    }

    [Fact]
    public void ToContract_FallsBackToUserNameWhenEmailIsNull()
    {
        var user = new ApplicationUser
        {
            Id = "user-2",
            Email = null,
            UserName = "fallback@example.com"
        };

        var contract = user.ToContract(Array.Empty<string>());

        Assert.Equal("fallback@example.com", contract.Email);
        Assert.Empty(contract.Roles);
    }
}
