using WarehouseERP.Shared.Contracts.PurchaseOrders;

namespace WarehouseERP.Blazor.Features.PurchaseOrders.Services;

public interface IPurchaseOrderApiClient
{
    Task<IReadOnlyList<PurchaseOrderDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PurchaseOrderDto>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderRequest request, CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto> AddLineAsync(Guid id, AddPurchaseOrderLineRequest request, CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto> UpdateLineAsync(Guid id, Guid productId, UpdatePurchaseOrderLineRequest request, CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto> RemoveLineAsync(Guid id, Guid productId, CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto> SubmitAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto> CancelAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto> ReceiveLineAsync(Guid id, Guid productId, ReceivePurchaseOrderLineRequest request, CancellationToken cancellationToken = default);
}
