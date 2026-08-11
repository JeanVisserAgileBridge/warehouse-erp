using System.Net.Http.Json;
using WarehouseERP.Blazor.Infrastructure.Http;
using WarehouseERP.Shared.Contracts.Customers;

namespace WarehouseERP.Blazor.Features.Customers.Services;

public sealed class CustomerApiClient : ICustomerApiClient
{
    private const string BaseRoute = "api/customers";

    private readonly HttpClient _httpClient;

    public CustomerApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BaseRoute, cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var customers = await response.Content.ReadFromJsonAsync<IReadOnlyList<CustomerDto>>(cancellationToken: cancellationToken);

        return customers ?? [];
    }

    public async Task<CustomerDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/{id}", cancellationToken);

        return await ReadCustomerAsync(response, cancellationToken);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, cancellationToken);

        return await ReadCustomerAsync(response, cancellationToken);
    }

    public async Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseRoute}/{id}", request, cancellationToken);

        return await ReadCustomerAsync(response, cancellationToken);
    }

    public async Task<CustomerDto> ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsync($"{BaseRoute}/{id}/activate", null, cancellationToken);

        return await ReadCustomerAsync(response, cancellationToken);
    }

    public async Task<CustomerDto> DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsync($"{BaseRoute}/{id}/deactivate", null, cancellationToken);

        return await ReadCustomerAsync(response, cancellationToken);
    }

    private static async Task<CustomerDto> ReadCustomerAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var customer = await response.Content.ReadFromJsonAsync<CustomerDto>(cancellationToken: cancellationToken);

        return customer ?? throw new ApiException((int)response.StatusCode, "The API returned an empty response.");
    }
}
