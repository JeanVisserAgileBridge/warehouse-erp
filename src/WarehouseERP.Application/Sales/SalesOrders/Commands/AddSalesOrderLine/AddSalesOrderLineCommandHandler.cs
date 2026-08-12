using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.ProductCatalog.Products;

namespace WarehouseERP.Application.Sales.SalesOrders.Commands.AddSalesOrderLine;

public sealed class AddSalesOrderLineCommandHandler : ICommandHandler<AddSalesOrderLineCommand, SalesOrderDto>
{
    private readonly ISalesOrderRepository _salesOrderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddSalesOrderLineCommandHandler(
        ISalesOrderRepository salesOrderRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _salesOrderRepository = salesOrderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SalesOrderDto> HandleAsync(AddSalesOrderLineCommand command, CancellationToken cancellationToken)
    {
        var salesOrder = await _salesOrderRepository.GetByIdAsync(command.SalesOrderId, cancellationToken)
            ?? throw new NotFoundException($"Sales order with id '{command.SalesOrderId}' was not found.");

        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken)
            ?? throw new NotFoundException($"Product with id '{command.ProductId}' was not found.");

        if (!product.IsActive)
        {
            throw new InactiveProductException($"Product with id '{command.ProductId}' is not active.");
        }

        salesOrder.AddLine(command.ProductId, command.QuantityOrdered, command.UnitPrice);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return SalesOrderDto.FromDomain(salesOrder);
    }
}
