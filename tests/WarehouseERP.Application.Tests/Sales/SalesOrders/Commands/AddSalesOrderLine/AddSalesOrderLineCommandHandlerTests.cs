using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Sales.SalesOrders.Commands.AddSalesOrderLine;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.ProductCatalog.Products.Fakes;
using WarehouseERP.Application.Tests.Sales.SalesOrders.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.ProductCatalog;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Tests.Sales.SalesOrders.Commands.AddSalesOrderLine;

public class AddSalesOrderLineCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_AddsLine_WhenProductExistsAndIsActive()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var productRepository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrderRepository.Seed(salesOrder);

        var product = Product.Create("SKU-1", "Widget", Guid.NewGuid(), 9.99m);
        productRepository.Seed(product);

        var handler = new AddSalesOrderLineCommandHandler(salesOrderRepository, productRepository, unitOfWork);

        var command = new AddSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
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
    public async Task HandleAsync_ThrowsNotFoundException_WhenSalesOrderDoesNotExist()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var productRepository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new AddSalesOrderLineCommandHandler(salesOrderRepository, productRepository, unitOfWork);

        var command = new AddSalesOrderLineCommand
        {
            SalesOrderId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            QuantityOrdered = 10,
            UnitPrice = 9.99m
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var productRepository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrderRepository.Seed(salesOrder);

        var handler = new AddSalesOrderLineCommandHandler(salesOrderRepository, productRepository, unitOfWork);

        var command = new AddSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = Guid.NewGuid(),
            QuantityOrdered = 10,
            UnitPrice = 9.99m
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsInactiveProductException_WhenProductIsNotActive()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var productRepository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrderRepository.Seed(salesOrder);

        var product = Product.Create("SKU-1", "Widget", Guid.NewGuid(), 9.99m);
        product.Deactivate();
        productRepository.Seed(product);

        var handler = new AddSalesOrderLineCommandHandler(salesOrderRepository, productRepository, unitOfWork);

        var command = new AddSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = product.Id,
            QuantityOrdered = 10,
            UnitPrice = 9.99m
        };

        await Assert.ThrowsAsync<InactiveProductException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenSalesOrderIsNotDraft()
    {
        var salesOrderRepository = new FakeSalesOrderRepository();
        var productRepository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        var firstProduct = Product.Create("SKU-1", "Widget", Guid.NewGuid(), 9.99m);
        productRepository.Seed(firstProduct);
        salesOrder.AddLine(firstProduct.Id, 5, 9.99m);
        salesOrder.Confirm();
        salesOrderRepository.Seed(salesOrder);

        var secondProduct = Product.Create("SKU-2", "Gadget", Guid.NewGuid(), 4.99m);
        productRepository.Seed(secondProduct);

        var handler = new AddSalesOrderLineCommandHandler(salesOrderRepository, productRepository, unitOfWork);

        var command = new AddSalesOrderLineCommand
        {
            SalesOrderId = salesOrder.Id,
            ProductId = secondProduct.Id,
            QuantityOrdered = 10,
            UnitPrice = 4.99m
        };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));
    }
}
