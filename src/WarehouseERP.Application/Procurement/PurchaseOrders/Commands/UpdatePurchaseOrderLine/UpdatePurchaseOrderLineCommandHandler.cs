using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.UpdatePurchaseOrderLine;

// There is no Domain "UpdateLine" behaviour. A line is edited by removing and re-adding it for
// the same ProductId, both of which already require the order to be Draft and re-validate
// quantity/price. Since a line can only be edited while Draft, QuantityReceived is always zero
// at this point, so nothing is lost by replacing it.
public sealed class UpdatePurchaseOrderLineCommandHandler : ICommandHandler<UpdatePurchaseOrderLineCommand, PurchaseOrderDto>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePurchaseOrderLineCommandHandler(IPurchaseOrderRepository purchaseOrderRepository, IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PurchaseOrderDto> HandleAsync(UpdatePurchaseOrderLineCommand command, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(command.PurchaseOrderId, cancellationToken)
            ?? throw new NotFoundException($"Purchase order with id '{command.PurchaseOrderId}' was not found.");

        purchaseOrder.RemoveLine(command.ProductId);
        purchaseOrder.AddLine(command.ProductId, command.QuantityOrdered, command.UnitPrice);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return PurchaseOrderDto.FromDomain(purchaseOrder);
    }
}
