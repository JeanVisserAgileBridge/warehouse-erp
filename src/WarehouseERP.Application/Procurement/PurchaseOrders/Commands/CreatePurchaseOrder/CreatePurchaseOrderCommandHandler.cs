using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.Suppliers;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.CreatePurchaseOrder;

public sealed class CreatePurchaseOrderCommandHandler : ICommandHandler<CreatePurchaseOrderCommand, PurchaseOrderDto>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePurchaseOrderCommandHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        ISupplierRepository supplierRepository,
        IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _supplierRepository = supplierRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PurchaseOrderDto> HandleAsync(CreatePurchaseOrderCommand command, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(command.SupplierId, cancellationToken)
            ?? throw new NotFoundException($"Supplier with id '{command.SupplierId}' was not found.");

        if (!supplier.IsActive)
        {
            throw new InactiveSupplierException($"Supplier with id '{command.SupplierId}' is not active.");
        }

        var existingOrder = await _purchaseOrderRepository.GetByOrderNumberAsync(command.OrderNumber, cancellationToken);
        if (existingOrder is not null)
        {
            throw new DuplicateOrderNumberException($"A purchase order numbered '{command.OrderNumber}' already exists.");
        }

        var purchaseOrder = PurchaseOrder.Create(command.SupplierId, command.OrderNumber, command.OrderDate, command.Notes);

        await _purchaseOrderRepository.AddAsync(purchaseOrder, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return PurchaseOrderDto.FromDomain(purchaseOrder);
    }
}
