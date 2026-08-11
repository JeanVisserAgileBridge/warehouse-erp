using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrderById;

public sealed class GetPurchaseOrderByIdQueryHandler : IQueryHandler<GetPurchaseOrderByIdQuery, PurchaseOrderDto>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;

    public GetPurchaseOrderByIdQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    public async Task<PurchaseOrderDto> HandleAsync(GetPurchaseOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException($"Purchase order with id '{query.Id}' was not found.");

        return PurchaseOrderDto.FromDomain(purchaseOrder);
    }
}
