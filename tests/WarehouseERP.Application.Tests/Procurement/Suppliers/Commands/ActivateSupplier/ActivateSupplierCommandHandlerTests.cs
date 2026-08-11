using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.Suppliers.Commands.ActivateSupplier;
using WarehouseERP.Application.Tests.Procurement.Suppliers.Fakes;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.Suppliers.Commands.ActivateSupplier;

public class ActivateSupplierCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ActivatesSupplier_WhenSupplierExists()
    {
        var supplierRepository = new FakeSupplierRepository();
        var supplier = Supplier.Create("Acme Supplies");
        supplier.Deactivate();
        supplierRepository.Seed(supplier);

        var handler = new ActivateSupplierCommandHandler(supplierRepository);

        var dto = await handler.HandleAsync(new ActivateSupplierCommand { Id = supplier.Id }, CancellationToken.None);

        Assert.True(dto.IsActive);
        Assert.True(supplier.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenSupplierDoesNotExist()
    {
        var supplierRepository = new FakeSupplierRepository();
        var handler = new ActivateSupplierCommandHandler(supplierRepository);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new ActivateSupplierCommand { Id = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToSupplierRepository()
    {
        var supplierRepository = new FakeSupplierRepository();
        var supplier = Supplier.Create("Acme Supplies");
        supplierRepository.Seed(supplier);

        var handler = new ActivateSupplierCommandHandler(supplierRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new ActivateSupplierCommand { Id = supplier.Id }, cts.Token);

        Assert.Equal(cts.Token, supplierRepository.LastCancellationToken);
    }
}
