using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrders;

public sealed class GetPurchaseOrdersQueryHandler : IQueryHandler<GetPurchaseOrdersQuery, IReadOnlyList<PurchaseOrderDto>>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;

    public GetPurchaseOrdersQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    public async Task<IReadOnlyList<PurchaseOrderDto>> HandleAsync(GetPurchaseOrdersQuery query, CancellationToken cancellationToken)
    {
        var purchaseOrders = await _purchaseOrderRepository.GetAllAsync(cancellationToken);

        return purchaseOrders.Select(PurchaseOrderDto.FromDomain).ToList();
    }
}
