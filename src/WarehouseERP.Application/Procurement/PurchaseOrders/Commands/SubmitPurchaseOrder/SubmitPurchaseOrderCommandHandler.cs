using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.SubmitPurchaseOrder;

public sealed class SubmitPurchaseOrderCommandHandler : ICommandHandler<SubmitPurchaseOrderCommand, PurchaseOrderDto>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitPurchaseOrderCommandHandler(IPurchaseOrderRepository purchaseOrderRepository, IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PurchaseOrderDto> HandleAsync(SubmitPurchaseOrderCommand command, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(command.PurchaseOrderId, cancellationToken)
            ?? throw new NotFoundException($"Purchase order with id '{command.PurchaseOrderId}' was not found.");

        purchaseOrder.Submit();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return PurchaseOrderDto.FromDomain(purchaseOrder);
    }
}
