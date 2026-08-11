using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.PurchaseOrders.Commands.AddPurchaseOrderLine;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.ProductCatalog.Products.Fakes;
using WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.ProductCatalog;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.PurchaseOrders.Commands.AddPurchaseOrderLine;

public class AddPurchaseOrderLineCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_AddsLine_WhenProductExistsAndIsActive()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var productRepository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrderRepository.Seed(purchaseOrder);

        var product = Product.Create("SKU-1", "Widget", Guid.NewGuid(), 9.99m);
        productRepository.Seed(product);

        var handler = new AddPurchaseOrderLineCommandHandler(purchaseOrderRepository, productRepository, unitOfWork);

        var command = new AddPurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = product.Id,
            QuantityOrdered = 10,
            UnitPrice = 9.99m
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        var line = Assert.Single(dto.Lines);
        Assert.Equal(product.Id, line.ProductId);
        Assert.Equal(10, line.QuantityOrdered);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenPurchaseOrderDoesNotExist()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var productRepository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new AddPurchaseOrderLineCommandHandler(purchaseOrderRepository, productRepository, unitOfWork);

        var command = new AddPurchaseOrderLineCommand
        {
            PurchaseOrderId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            QuantityOrdered = 10,
            UnitPrice = 9.99m
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var productRepository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrderRepository.Seed(purchaseOrder);

        var handler = new AddPurchaseOrderLineCommandHandler(purchaseOrderRepository, productRepository, unitOfWork);

        var command = new AddPurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = Guid.NewGuid(),
            QuantityOrdered = 10,
            UnitPrice = 9.99m
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsInactiveProductException_WhenProductIsNotActive()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var productRepository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        purchaseOrderRepository.Seed(purchaseOrder);

        var product = Product.Create("SKU-1", "Widget", Guid.NewGuid(), 9.99m);
        product.Deactivate();
        productRepository.Seed(product);

        var handler = new AddPurchaseOrderLineCommandHandler(purchaseOrderRepository, productRepository, unitOfWork);

        var command = new AddPurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = product.Id,
            QuantityOrdered = 10,
            UnitPrice = 9.99m
        };

        await Assert.ThrowsAsync<InactiveProductException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenPurchaseOrderIsNotDraft()
    {
        var purchaseOrderRepository = new FakePurchaseOrderRepository();
        var productRepository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), "PO-001", DateTime.UtcNow);
        var firstProduct = Product.Create("SKU-1", "Widget", Guid.NewGuid(), 9.99m);
        productRepository.Seed(firstProduct);
        purchaseOrder.AddLine(firstProduct.Id, 5, 9.99m);
        purchaseOrder.Submit();
        purchaseOrderRepository.Seed(purchaseOrder);

        var secondProduct = Product.Create("SKU-2", "Gadget", Guid.NewGuid(), 4.99m);
        productRepository.Seed(secondProduct);

        var handler = new AddPurchaseOrderLineCommandHandler(purchaseOrderRepository, productRepository, unitOfWork);

        var command = new AddPurchaseOrderLineCommand
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = secondProduct.Id,
            QuantityOrdered = 10,
            UnitPrice = 4.99m
        };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));
    }
}
