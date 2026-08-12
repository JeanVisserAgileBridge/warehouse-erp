using WarehouseERP.Shared.Contracts.SalesOrders;

namespace WarehouseERP.Blazor.Features.SalesOrders.Services;

public interface ISalesOrderApiClient
{
    Task<IReadOnlyList<SalesOrderDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<SalesOrderDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SalesOrderDto>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);

    Task<SalesOrderDto> CreateAsync(CreateSalesOrderRequest request, CancellationToken cancellationToken = default);

    Task<SalesOrderDto> AddLineAsync(Guid id, AddSalesOrderLineRequest request, CancellationToken cancellationToken = default);

    Task<SalesOrderDto> UpdateLineAsync(Guid id, Guid productId, UpdateSalesOrderLineRequest request, CancellationToken cancellationToken = default);

    Task<SalesOrderDto> RemoveLineAsync(Guid id, Guid productId, CancellationToken cancellationToken = default);

    Task<SalesOrderDto> ConfirmAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SalesOrderDto> CancelAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SalesOrderDto> FulfilLineAsync(Guid id, Guid productId, FulfilSalesOrderLineRequest request, CancellationToken cancellationToken = default);
}
