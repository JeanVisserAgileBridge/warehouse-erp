using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.SupplierTests;

public class SupplierRenameTests
{
    [Fact]
    public void Rename_UpdatesName()
    {
        var supplier = Supplier.Create("Acme Supplies");

        supplier.Rename("Acme Industrial");

        Assert.Equal("Acme Industrial", supplier.Name);
    }

    [Fact]
    public void Rename_UpdatesUpdatedAt()
    {
        var supplier = Supplier.Create("Acme Supplies");
        var originalUpdatedAt = supplier.UpdatedAt;

        supplier.Rename("Acme Industrial");

        Assert.True(supplier.UpdatedAt >= originalUpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Rename_RejectsNullEmptyOrWhitespaceName(string? name)
    {
        var supplier = Supplier.Create("Acme Supplies");

        Assert.Throws<DomainException>(() => supplier.Rename(name!));
    }

    [Fact]
    public void Rename_RejectsNameLongerThanMaxLength()
    {
        var supplier = Supplier.Create("Acme Supplies");
        var name = new string('a', Supplier.MaxNameLength + 1);

        Assert.Throws<DomainException>(() => supplier.Rename(name));
    }
}
