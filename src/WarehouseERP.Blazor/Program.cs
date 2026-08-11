using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WarehouseERP.Blazor;
using WarehouseERP.Blazor.Configuration;
using WarehouseERP.Blazor.Features.Categories.Services;
using WarehouseERP.Blazor.Features.Customers.Services;
using WarehouseERP.Blazor.Features.Dashboard.Services;
using WarehouseERP.Blazor.Features.Products.Services;
using WarehouseERP.Blazor.Features.StorageLocations.Services;
using WarehouseERP.Blazor.Features.Suppliers.Services;
using WarehouseERP.Blazor.Features.Warehouses.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiOptions = builder.Configuration.GetSection(ApiOptions.SectionName).Get<ApiOptions>()
    ?? throw new InvalidOperationException($"Missing configuration section '{ApiOptions.SectionName}'.");

builder.Services.AddSingleton(apiOptions);

builder.Services.AddHttpClient<ICategoryApiClient, CategoryApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
});

builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
});

builder.Services.AddHttpClient<IDashboardApiClient, DashboardApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
});

builder.Services.AddHttpClient<ISupplierApiClient, SupplierApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
});

builder.Services.AddHttpClient<ICustomerApiClient, CustomerApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
});

builder.Services.AddHttpClient<IWarehouseApiClient, WarehouseApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
});

builder.Services.AddHttpClient<IStorageLocationApiClient, StorageLocationApiClient>(client =>
{
    client.BaseAddress = new Uri(apiOptions.BaseUrl);
});

await builder.Build().RunAsync();
