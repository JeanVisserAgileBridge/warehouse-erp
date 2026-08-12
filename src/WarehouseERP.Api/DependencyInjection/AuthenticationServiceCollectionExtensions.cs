using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WarehouseERP.Infrastructure.Identity;

namespace WarehouseERP.Api.DependencyInjection;

public static class AuthenticationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationIdentity(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

        // This app only ever authenticates the Blazor browser client via a cookie, never via
        // a bearer token, so the default scheme is switched from AddIdentityApiEndpoints'
        // default (Bearer) to the cookie scheme it also registers.
        services.AddAuthentication(IdentityConstants.ApplicationScheme);

        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = true;

            // The Blazor client and this API run on different origins (different ports in
            // development), so the cookie must be usable cross-site; SameSite=None requires Secure.
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            // This is a JSON API, not a page-rendering app — return status codes instead of
            // redirecting an unauthenticated/forbidden request to a login page that doesn't exist.
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
        });

        services.AddScoped<RoleSeeder>();
        services.AddScoped<IdentitySeeder>();

        return services;
    }
}
