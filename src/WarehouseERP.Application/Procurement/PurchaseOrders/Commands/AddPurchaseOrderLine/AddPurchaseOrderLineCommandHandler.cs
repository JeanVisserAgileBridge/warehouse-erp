using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.ProductCatalog.Products;

namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.AddPurchaseOrderLine;

public sealed class AddPurchaseOrderLineCommandHandler : ICommandHandler<AddPurchaseOrderLineCommand, PurchaseOrderDto>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPurchaseOrderLineCommandHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PurchaseOrderDto> HandleAsync(AddPurchaseOrderLineCommand command, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(command.PurchaseOrderId, cancellationToken)
            ?? throw new NotFoundException($"Purchase order with id '{command.PurchaseOrderId}' was not found.");

        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken)
            ?? throw new NotFoundException($"Product with id '{command.ProductId}' was not found.");

        if (!product.IsActive)
        {
            throw new InactiveProductException($"Product with id '{command.ProductId}' is not active.");
        }

        purchaseOrder.AddLine(command.ProductId, command.QuantityOrdered, command.UnitPrice);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return PurchaseOrderDto.FromDomain(purchaseOrder);
    }
}
