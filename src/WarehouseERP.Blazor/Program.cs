using System.Globalization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WarehouseERP.Blazor;
using WarehouseERP.Blazor.Configuration;
using WarehouseERP.Blazor.Features.Admin.Users.Services;
using WarehouseERP.Blazor.Features.Auth.Services;
using WarehouseERP.Blazor.Features.Categories.Services;
using WarehouseERP.Blazor.Features.Customers.Services;
using WarehouseERP.Blazor.Features.Dashboard.Services;
using WarehouseERP.Blazor.Features.Inventory.Services;
using WarehouseERP.Blazor.Features.Products.Services;
using WarehouseERP.Blazor.Features.PurchaseOrders.Services;
using WarehouseERP.Blazor.Features.SalesOrders.Services;
using WarehouseERP.Blazor.Features.StorageLocations.Services;
using WarehouseERP.Blazor.Features.Suppliers.Services;
using WarehouseERP.Blazor.Features.Warehouses.Services;
using WarehouseERP.Blazor.Infrastructure.Auth;

// South African culture, applied globally so existing and future currency/date formatting
// (e.g. ToString("C"), ToString("d"), ToString("g")) is correct without hardcoding "R" or
// custom format strings on individual pages. .NET's built-in en-ZA ShortDatePattern is
// "yyyy/MM/dd"; overriding it to "dd/MM/yyyy" is the only change needed, since ShortTimePattern
// ("HH:mm") already matches South African conventions. The Blazor WebAssembly runtime's bundled
// ICU/CLDR data reports en-ZA's currency symbol as "ZAR" rather than "R", so that is overridden
// too, for the same reason as the date pattern.
var southAfricanCulture = new CultureInfo("en-ZA");
southAfricanCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
southAfricanCulture.NumberFormat.CurrencySymbol = "R";
CultureInfo.DefaultThreadCurrentCulture = southAfricanCulture;
CultureInfo.DefaultThreadCurrentUICulture = southAfricanCulture;
CultureInfo.CurrentCulture = southAfricanCulture;
CultureInfo.CurrentUICulture = southAfricanCulture;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiOptions = builder.Configuration.GetSection(ApiOptions.SectionName).Get<ApiOptions>()
    ?? throw new InvalidOperationException($"Missing configuration section '{ApiOptions.SectionName}'.");

builder.Services.AddSingleton(apiOptions);

builder.Services.AddTransient<CookieCredentialsHandler>();

// Route protection comes from [Authorize] applied to every component via the root
// _Imports.razor (AuthorizeRouteView only ever looks at attributes on the routed page type,
// not at any fallback policy); Login.razor opts out explicitly with [AllowAnonymous].
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CookieAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CookieAuthenticationStateProvider>());

builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
}).AddHttpMessageHandler<CookieCredentialsHandler>();

builder.Services.AddHttpClient<IAdminUserApiClient, AdminUserApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
}).AddHttpMessageHandler<CookieCredentialsHandler>();

builder.Services.AddHttpClient<ICategoryApiClient, CategoryApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
}).AddHttpMessageHandler<CookieCredentialsHandler>();

builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
}).AddHttpMessageHandler<CookieCredentialsHandler>();

builder.Services.AddHttpClient<IDashboardApiClient, DashboardApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
}).AddHttpMessageHandler<CookieCredentialsHandler>();

builder.Services.AddHttpClient<ISupplierApiClient, SupplierApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
}).AddHttpMessageHandler<CookieCredentialsHandler>();

builder.Services.AddHttpClient<ICustomerApiClient, CustomerApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
}).AddHttpMessageHandler<CookieCredentialsHandler>();

builder.Services.AddHttpClient<IWarehouseApiClient, WarehouseApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
}).AddHttpMessageHandler<CookieCredentialsHandler>();

builder.Services.AddHttpClient<IStorageLocationApiClient, StorageLocationApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
}).AddHttpMessageHandler<CookieCredentialsHandler>();

builder.Services.AddHttpClient<IInventoryApiClient, InventoryApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
}).AddHttpMessageHandler<CookieCredentialsHandler>();

builder.Services.AddHttpClient<IPurchaseOrderApiClient, PurchaseOrderApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
}).AddHttpMessageHandler<CookieCredentialsHandler>();

builder.Services.AddHttpClient<ISalesOrderApiClient, SalesOrderApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
}).AddHttpMessageHandler<CookieCredentialsHandler>();

await builder.Build().RunAsync();
