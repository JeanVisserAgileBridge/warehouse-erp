using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.Suppliers.Commands.DeactivateSupplier;
using WarehouseERP.Application.Tests.Procurement.Suppliers.Fakes;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.Suppliers.Commands.DeactivateSupplier;

public class DeactivateSupplierCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_DeactivatesSupplier_WhenSupplierExists()
    {
        var supplierRepository = new FakeSupplierRepository();
        var supplier = Supplier.Create("Acme Supplies");
        supplierRepository.Seed(supplier);

        var handler = new DeactivateSupplierCommandHandler(supplierRepository);

        var dto = await handler.HandleAsync(new DeactivateSupplierCommand { Id = supplier.Id }, CancellationToken.None);

        Assert.False(dto.IsActive);
        Assert.False(supplier.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenSupplierDoesNotExist()
    {
        var supplierRepository = new FakeSupplierRepository();
        var handler = new DeactivateSupplierCommandHandler(supplierRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new DeactivateSupplierCommand { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToSupplierRepository()
    {
        var supplierRepository = new FakeSupplierRepository();
        var supplier = Supplier.Create("Acme Supplies");
        supplierRepository.Seed(supplier);

        var handler = new DeactivateSupplierCommandHandler(supplierRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new DeactivateSupplierCommand { Id = supplier.Id }, cts.Token);

        Assert.Equal(cts.Token, supplierRepository.LastCancellationToken);
    }
}
