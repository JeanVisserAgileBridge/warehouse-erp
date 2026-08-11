using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrdersBySupplierId;

public sealed class GetPurchaseOrdersBySupplierIdQueryHandler
    : IQueryHandler<GetPurchaseOrdersBySupplierIdQuery, IReadOnlyList<PurchaseOrderDto>>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;

    public GetPurchaseOrdersBySupplierIdQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    public async Task<IReadOnlyList<PurchaseOrderDto>> HandleAsync(
        GetPurchaseOrdersBySupplierIdQuery query, CancellationToken cancellationToken)
    {
        var purchaseOrders = await _purchaseOrderRepository.GetBySupplierIdAsync(query.SupplierId, cancellationToken);

        return purchaseOrders.Select(PurchaseOrderDto.FromDomain).ToList();
    }
}
