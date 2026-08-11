using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.Suppliers.Commands.UpdateSupplier;
using WarehouseERP.Application.Tests.Procurement.Suppliers.Fakes;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_UpdatesSupplier_WhenValid()
    {
        var supplierRepository = new FakeSupplierRepository();
        var supplier = Supplier.Create("Acme Supplies");
        supplierRepository.Seed(supplier);

        var handler = new UpdateSupplierCommandHandler(supplierRepository);

        var command = new UpdateSupplierCommand
        {
            Id = supplier.Id,
            Name = "Acme Supplies Updated",
            Email = "updated@acme.test",
            PhoneNumber = "555-0199",
            Address = "2 Warehouse Way"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("Acme Supplies Updated", dto.Name);
        Assert.Equal("updated@acme.test", dto.Email);
        Assert.Equal("555-0199", dto.PhoneNumber);
        Assert.Equal("2 Warehouse Way", dto.Address);
        Assert.Equal("Acme Supplies Updated", supplier.Name);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenSupplierDoesNotExist()
    {
        var supplierRepository = new FakeSupplierRepository();
        var handler = new UpdateSupplierCommandHandler(supplierRepository);

        var command = new UpdateSupplierCommand
        {
            Id = Guid.NewGuid(),
            Name = "Acme Supplies"
        };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateNameException_WhenNameBelongsToAnotherSupplier()
    {
        var supplierRepository = new FakeSupplierRepository();
        var supplierToUpdate = Supplier.Create("Acme Supplies");
        var otherSupplier = Supplier.Create("Globex Supplies");
        supplierRepository.Seed(supplierToUpdate);
        supplierRepository.Seed(otherSupplier);

        var handler = new UpdateSupplierCommandHandler(supplierRepository);

        var command = new UpdateSupplierCommand
        {
            Id = supplierToUpdate.Id,
            Name = "globex supplies"
        };

        await Assert.ThrowsAsync<DuplicateNameException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_AllowsUpdate_WhenNameIsUnchanged()
    {
        var supplierRepository = new FakeSupplierRepository();
        var supplier = Supplier.Create("Acme Supplies");
        supplierRepository.Seed(supplier);

        var handler = new UpdateSupplierCommandHandler(supplierRepository);

        var command = new UpdateSupplierCommand
        {
            Id = supplier.Id,
            Name = "Acme Supplies",
            PhoneNumber = "555-0199"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("Acme Supplies", dto.Name);
        Assert.Equal("555-0199", dto.PhoneNumber);
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToSupplierRepository()
    {
        var supplierRepository = new FakeSupplierRepository();
        var supplier = Supplier.Create("Acme Supplies");
        supplierRepository.Seed(supplier);

        var handler = new UpdateSupplierCommandHandler(supplierRepository);

        var command = new UpdateSupplierCommand
        {
            Id = supplier.Id,
            Name = "Acme Supplies"
        };

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(command, cts.Token);

        Assert.Equal(cts.Token, supplierRepository.LastCancellationToken);
    }
}
