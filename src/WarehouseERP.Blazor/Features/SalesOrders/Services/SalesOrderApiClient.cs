using System.Net.Http.Json;
using WarehouseERP.Blazor.Infrastructure.Http;
using WarehouseERP.Shared.Contracts.SalesOrders;

namespace WarehouseERP.Blazor.Features.SalesOrders.Services;

public sealed class SalesOrderApiClient : ISalesOrderApiClient
{
    private const string BaseRoute = "api/sales-orders";

    private readonly HttpClient _httpClient;

    public SalesOrderApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<SalesOrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BaseRoute, cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var salesOrders = await response.Content.ReadFromJsonAsync<IReadOnlyList<SalesOrderDto>>(cancellationToken: cancellationToken);

        return salesOrders ?? [];
    }

    public async Task<SalesOrderDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/{id}", cancellationToken);

        return await ReadSalesOrderAsync(response, cancellationToken);
    }

    public async Task<IReadOnlyList<SalesOrderDto>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/customers/{customerId}/sales-orders", cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var salesOrders = await response.Content.ReadFromJsonAsync<IReadOnlyList<SalesOrderDto>>(cancellationToken: cancellationToken);

        return salesOrders ?? [];
    }

    public async Task<SalesOrderDto> CreateAsync(CreateSalesOrderRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, cancellationToken);

        return await ReadSalesOrderAsync(response, cancellationToken);
    }

    public async Task<SalesOrderDto> AddLineAsync(Guid id, AddSalesOrderLineRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseRoute}/{id}/lines", request, cancellationToken);

        return await ReadSalesOrderAsync(response, cancellationToken);
    }

    public async Task<SalesOrderDto> UpdateLineAsync(Guid id, Guid productId, UpdateSalesOrderLineRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseRoute}/{id}/lines/{productId}", request, cancellationToken);

        return await ReadSalesOrderAsync(response, cancellationToken);
    }

    public async Task<SalesOrderDto> RemoveLineAsync(Guid id, Guid productId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"{BaseRoute}/{id}/lines/{productId}", cancellationToken);

        return await ReadSalesOrderAsync(response, cancellationToken);
    }

    public async Task<SalesOrderDto> ConfirmAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync($"{BaseRoute}/{id}/confirm", null, cancellationToken);

        return await ReadSalesOrderAsync(response, cancellationToken);
    }

    public async Task<SalesOrderDto> CancelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync($"{BaseRoute}/{id}/cancel", null, cancellationToken);

        return await ReadSalesOrderAsync(response, cancellationToken);
    }

    public async Task<SalesOrderDto> FulfilLineAsync(Guid id, Guid productId, FulfilSalesOrderLineRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseRoute}/{id}/lines/{productId}/fulfil", request, cancellationToken);

        return await ReadSalesOrderAsync(response, cancellationToken);
    }

    private static async Task<SalesOrderDto> ReadSalesOrderAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var salesOrder = await response.Content.ReadFromJsonAsync<SalesOrderDto>(cancellationToken: cancellationToken);

        return salesOrder ?? throw new ApiException((int)response.StatusCode, "The API returned an empty response.");
    }
}
