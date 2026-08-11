using WarehouseERP.Application.Procurement.Suppliers.Queries.GetSuppliers;
using WarehouseERP.Application.Tests.Procurement.Suppliers.Fakes;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Tests.Procurement.Suppliers.Queries.GetSuppliers;

public class GetSuppliersQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsAllSuppliersAsDtos()
    {
        var supplierRepository = new FakeSupplierRepository();
        var first = Supplier.Create("Acme Supplies");
        var second = Supplier.Create("Globex Supplies");
        supplierRepository.Seed(first);
        supplierRepository.Seed(second);

        var handler = new GetSuppliersQueryHandler(supplierRepository);

        var dtos = await handler.HandleAsync(new GetSuppliersQuery(), CancellationToken.None);

        Assert.Equal(2, dtos.Count);
        Assert.Contains(dtos, d => d.Name == "Acme Supplies");
        Assert.Contains(dtos, d => d.Name == "Globex Supplies");
    }

    [Fact]
    public async Task HandleAsync_ReturnsEmptyList_WhenNoSuppliersExist()
    {
        var supplierRepository = new FakeSupplierRepository();
        var handler = new GetSuppliersQueryHandler(supplierRepository);

        var dtos = await handler.HandleAsync(new GetSuppliersQuery(), CancellationToken.None);

        Assert.Empty(dtos);
    }

    [Fact]
    public async Task HandleAsync_PropagatesCancellationToken_ToSupplierRepository()
    {
        var supplierRepository = new FakeSupplierRepository();
        var handler = new GetSuppliersQueryHandler(supplierRepository);

        using var cts = new CancellationTokenSource();

        await handler.HandleAsync(new GetSuppliersQuery(), cts.Token);

        Assert.Equal(cts.Token, supplierRepository.LastCancellationToken);
    }
}
