using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using WarehouseERP.Api.DependencyInjection;
using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Api.Tests.Authorization;

public class PolicyConfigurationTests
{
    [Theory]
    [InlineData(PolicyNames.AdminOnly, new[] { RoleNames.Admin })]
    [InlineData(PolicyNames.WarehouseAccess, new[] { RoleNames.Admin, RoleNames.Warehouse })]
    [InlineData(PolicyNames.PurchasingAccess, new[] { RoleNames.Admin, RoleNames.Purchasing })]
    [InlineData(PolicyNames.SalesAccess, new[] { RoleNames.Admin, RoleNames.Sales })]
    public async Task Policy_RequiresExpectedRoles(string policyName, string[] expectedRoles)
    {
        var provider = BuildAuthorizationPolicyProvider();

        var policy = await provider.GetPolicyAsync(policyName);

        Assert.NotNull(policy);
        var requirement = Assert.Single(policy.Requirements.OfType<RolesAuthorizationRequirement>());
        Assert.Equal(expectedRoles.OrderBy(r => r), requirement.AllowedRoles.OrderBy(r => r));
    }

    [Fact]
    public async Task AdminOnly_DoesNotAllowFunctionalRolesAlone()
    {
        var provider = BuildAuthorizationPolicyProvider();

        var policy = await provider.GetPolicyAsync(PolicyNames.AdminOnly);

        Assert.NotNull(policy);
        var requirement = Assert.Single(policy.Requirements.OfType<RolesAuthorizationRequirement>());
        Assert.DoesNotContain(RoleNames.Warehouse, requirement.AllowedRoles);
        Assert.DoesNotContain(RoleNames.Purchasing, requirement.AllowedRoles);
        Assert.DoesNotContain(RoleNames.Sales, requirement.AllowedRoles);
    }

    private static IAuthorizationPolicyProvider BuildAuthorizationPolicyProvider()
    {
        var services = new ServiceCollection();
        services.AddErpAuthorizationPolicies();

        return services.BuildServiceProvider().GetRequiredService<IAuthorizationPolicyProvider>();
    }
}
