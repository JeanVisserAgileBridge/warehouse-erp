using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.Suppliers.Queries.GetSupplierById;
using WarehouseERP.Application.Tests.Procurement.Suppliers.Fakes;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsMatchingSupplierDto_WhenSupplierExists()
    {
        var supplierRepository = new FakeSupplierRepository();
        var supplier = Supplier.Create("Acme Supplies", "contact@acme.test", "555-0100", "1 Warehouse Way");
        supplierRepository.Seed(supplier);

        var handler = new GetSupplierByIdQueryHandler(supplierRepository);

        var dto = await handler.HandleAsync(new GetSupplierByIdQuery { Id = supplier.Id }, CancellationToken.None);

        Assert.Equal(supplier.Id, dto.Id);
        Assert.Equal(supplier.Name, dto.Name);
        Assert.Equal(supplier.Email, dto.Email);
        Assert.Equal(supplier.PhoneNumber, dto.PhoneNumber);
        Assert.Equal(supplier.Address, dto.Address);
        Assert.Equal(supplier.IsActive, dto.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenSupplierDoesNotExist()
    {
        var supplierRepository = new FakeSupplierRepository();
        var handler = new GetSupplierByIdQueryHandler(supplierRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new GetSupplierByIdQuery { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToSupplierRepository()
    {
        var supplierRepository = new FakeSupplierRepository();
        var supplier = Supplier.Create("Acme Supplies");
        supplierRepository.Seed(supplier);

        var handler = new GetSupplierByIdQueryHandler(supplierRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new GetSupplierByIdQuery { Id = supplier.Id }, cts.Token);

        Assert.Equal(cts.Token, supplierRepository.LastCancellationToken);
    }
}
