using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.RemovePurchaseOrderLine;

public sealed class RemovePurchaseOrderLineCommandHandler : ICommandHandler<RemovePurchaseOrderLineCommand, PurchaseOrderDto>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemovePurchaseOrderLineCommandHandler(IPurchaseOrderRepository purchaseOrderRepository, IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PurchaseOrderDto> HandleAsync(RemovePurchaseOrderLineCommand command, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(command.PurchaseOrderId, cancellationToken)
            ?? throw new NotFoundException($"Purchase order with id '{command.PurchaseOrderId}' was not found.");

        purchaseOrder.RemoveLine(command.ProductId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return PurchaseOrderDto.FromDomain(purchaseOrder);
    }
}
