using WarehouseERP.Shared.Contracts.Auth;

namespace WarehouseERP.Api.DependencyInjection;

public static class AuthorizationPolicyServiceCollectionExtensions
{
    public static IServiceCollection AddErpAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.AdminOnly, policy => policy.RequireRole(RoleNames.Admin))
            .AddPolicy(PolicyNames.WarehouseAccess, policy => policy.RequireRole(RoleNames.Admin, RoleNames.Warehouse))
            .AddPolicy(PolicyNames.PurchasingAccess, policy => policy.RequireRole(RoleNames.Admin, RoleNames.Purchasing))
            .AddPolicy(PolicyNames.SalesAccess, policy => policy.RequireRole(RoleNames.Admin, RoleNames.Sales));

        return services;
    }
}
