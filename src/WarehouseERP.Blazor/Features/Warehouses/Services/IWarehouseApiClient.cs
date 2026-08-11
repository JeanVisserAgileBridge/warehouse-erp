using WarehouseERP.Shared.Contracts.Warehouses;

namespace WarehouseERP.Blazor.Features.Warehouses.Services;

public interface IWarehouseApiClient
{
    Task<IReadOnlyList<WarehouseDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<WarehouseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<WarehouseDto> CreateAsync(CreateWarehouseRequest request, CancellationToken cancellationToken = default);

    Task<WarehouseDto> UpdateAsync(Guid id, UpdateWarehouseRequest request, CancellationToken cancellationToken = default);

    Task<WarehouseDto> ActivateAsync(Guid id, CancellationToken cancellationToken = default);

    Task<WarehouseDto> DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
}
