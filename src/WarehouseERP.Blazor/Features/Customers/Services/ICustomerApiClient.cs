using WarehouseERP.Shared.Contracts.Customers;

namespace WarehouseERP.Blazor.Features.Customers.Services;

public interface ICustomerApiClient
{
    Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CustomerDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);

    Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken = default);

    Task<CustomerDto> ActivateAsync(Guid id, CancellationToken cancellationToken = default);

    Task<CustomerDto> DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
}
