using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Procurement.Suppliers.Commands.CreateSupplier;
using WarehouseERP.Application.Tests.Procurement.Suppliers.Fakes;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_AddsSupplierToRepository_WhenNameIsUnique()
    {
        var supplierRepository = new FakeSupplierRepository();
        var handler = new CreateSupplierCommandHandler(supplierRepository);

        var command = new CreateSupplierCommand
        {
            Name = "Acme Supplies"
        };

        await handler.HandleAsync(command, CancellationToken.None);

        var suppliers = await supplierRepository.GetAllAsync(CancellationToken.None);
        Assert.Single(suppliers);
    }

    [Fact]
    public async Task HandleAsync_ReturnsMatchingSupplierDto_WhenValid()
    {
        var supplierRepository = new FakeSupplierRepository();
        var handler = new CreateSupplierCommandHandler(supplierRepository);

        var command = new CreateSupplierCommand
        {
            Name = "Acme Supplies",
            Email = "contact@acme.test",
            PhoneNumber = "555-0100",
            Address = "1 Warehouse Way"
        };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal("Acme Supplies", dto.Name);
        Assert.Equal("contact@acme.test", dto.Email);
        Assert.Equal("555-0100", dto.PhoneNumber);
        Assert.Equal("1 Warehouse Way", dto.Address);
        Assert.True(dto.IsActive);
    }

    [Fact]
    public async Task HandleAsync_ThrowsDuplicateNameException_WhenNameAlreadyExistsWithDifferentCase()
    {
        var supplierRepository = new FakeSupplierRepository();
        supplierRepository.Seed(Supplier.Create("acme supplies"));

        var handler = new CreateSupplierCommandHandler(supplierRepository);

        var command = new CreateSupplierCommand
        {
            Name = "Acme Supplies"
        };

        await Assert.ThrowsAsync<DuplicateNameException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToSupplierRepository()
    {
        var supplierRepository = new FakeSupplierRepository();
        var handler = new CreateSupplierCommandHandler(supplierRepository);

        var command = new CreateSupplierCommand
        {
            Name = "Acme Supplies"
        };

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(command, cts.Token);

        Assert.Equal(cts.Token, supplierRepository.LastCancellationToken);
    }
}
