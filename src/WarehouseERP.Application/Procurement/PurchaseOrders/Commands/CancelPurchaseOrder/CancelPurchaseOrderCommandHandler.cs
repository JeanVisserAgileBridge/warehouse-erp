using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.CancelPurchaseOrder;

public sealed class CancelPurchaseOrderCommandHandler : ICommandHandler<CancelPurchaseOrderCommand, PurchaseOrderDto>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelPurchaseOrderCommandHandler(IPurchaseOrderRepository purchaseOrderRepository, IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PurchaseOrderDto> HandleAsync(CancelPurchaseOrderCommand command, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(command.PurchaseOrderId, cancellationToken)
            ?? throw new NotFoundException($"Purchase order with id '{command.PurchaseOrderId}' was not found.");

        purchaseOrder.Cancel();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return PurchaseOrderDto.FromDomain(purchaseOrder);
    }
}
