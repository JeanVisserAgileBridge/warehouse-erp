using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.SupplierTests;

public class SupplierChangeAddressTests
{
    [Fact]
    public void ChangeAddress_UpdatesAddress()
    {
        var supplier = Supplier.Create("Acme Supplies", address: "1 Industrial Way");

        supplier.ChangeAddress("2 Commerce Street");

        Assert.Equal("2 Commerce Street", supplier.Address);
    }

    [Fact]
    public void ChangeAddress_ClearsAddressWhenNull()
    {
        var supplier = Supplier.Create("Acme Supplies", address: "1 Industrial Way");

        supplier.ChangeAddress(null);

        Assert.Null(supplier.Address);
    }

    [Fact]
    public void ChangeAddress_UpdatesUpdatedAt()
    {
        var supplier = Supplier.Create("Acme Supplies");
        var originalUpdatedAt = supplier.UpdatedAt;

        supplier.ChangeAddress("2 Commerce Street");

        Assert.True(supplier.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ChangeAddress_RejectsAddressLongerThanMaxLength()
    {
        var supplier = Supplier.Create("Acme Supplies");
        var address = new string('a', Supplier.MaxAddressLength + 1);

        Assert.Throws<DomainException>(() => supplier.ChangeAddress(address));
    }

    [Fact]
    public void ChangeAddress_AcceptsAddressAtMaxLength()
    {
        var supplier = Supplier.Create("Acme Supplies");
        var address = new string('a', Supplier.MaxAddressLength);

        supplier.ChangeAddress(address);

        Assert.Equal(address, supplier.Address);
    }
}
