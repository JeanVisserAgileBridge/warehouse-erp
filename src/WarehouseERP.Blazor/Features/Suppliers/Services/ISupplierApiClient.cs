using WarehouseERP.Shared.Contracts.Suppliers;

namespace WarehouseERP.Blazor.Features.Suppliers.Services;

public interface ISupplierApiClient
{
    Task<IReadOnlyList<SupplierDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<SupplierDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SupplierDto> CreateAsync(CreateSupplierRequest request, CancellationToken cancellationToken = default);

    Task<SupplierDto> UpdateAsync(Guid id, UpdateSupplierRequest request, CancellationToken cancellationToken = default);

    Task<SupplierDto> ActivateAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SupplierDto> DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
}
