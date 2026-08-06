using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.SupplierTests;

public class SupplierActivationTests
{
    [Fact]
    public void Deactivate_MakesSupplierInactive()
    {
        var supplier = Supplier.Create("Acme Supplies");

        supplier.Deactivate();

        Assert.False(supplier.IsActive);
    }

    [Fact]
    public void Deactivate_IsIdempotent()
    {
        var supplier = Supplier.Create("Acme Supplies");

        supplier.Deactivate();
        supplier.Deactivate();

        Assert.False(supplier.IsActive);
    }

    [Fact]
    public void Deactivate_UpdatesUpdatedAt()
    {
        var supplier = Supplier.Create("Acme Supplies");
        var originalUpdatedAt = supplier.UpdatedAt;

        supplier.Deactivate();

        Assert.True(supplier.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void Activate_MakesInactiveSupplierActive()
    {
        var supplier = Supplier.Create("Acme Supplies");
        supplier.Deactivate();

        supplier.Activate();

        Assert.True(supplier.IsActive);
    }

    [Fact]
    public void Activate_IsIdempotent()
    {
        var supplier = Supplier.Create("Acme Supplies");

        supplier.Activate();
        supplier.Activate();

        Assert.True(supplier.IsActive);
    }

    [Fact]
    public void Activate_UpdatesUpdatedAt()
    {
        var supplier = Supplier.Create("Acme Supplies");
        supplier.Deactivate();
        var originalUpdatedAt = supplier.UpdatedAt;

        supplier.Activate();

        Assert.True(supplier.UpdatedAt >= originalUpdatedAt);
    }
}
