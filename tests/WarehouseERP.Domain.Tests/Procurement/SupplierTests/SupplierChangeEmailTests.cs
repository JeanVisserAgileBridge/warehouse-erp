using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.SupplierTests;

public class SupplierChangeEmailTests
{
    [Fact]
    public void ChangeEmail_UpdatesEmail()
    {
        var supplier = Supplier.Create("Acme Supplies", "old@acme.com");

        supplier.ChangeEmail("new@acme.com");

        Assert.Equal("new@acme.com", supplier.Email);
    }

    [Fact]
    public void ChangeEmail_ClearsEmailWhenNull()
    {
        var supplier = Supplier.Create("Acme Supplies", "old@acme.com");

        supplier.ChangeEmail(null);

        Assert.Null(supplier.Email);
    }

    [Fact]
    public void ChangeEmail_UpdatesUpdatedAt()
    {
        var supplier = Supplier.Create("Acme Supplies");
        var originalUpdatedAt = supplier.UpdatedAt;

        supplier.ChangeEmail("new@acme.com");

        Assert.True(supplier.UpdatedAt >= originalUpdatedAt);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("missing-at-sign.com")]
    [InlineData("@missing-local.com")]
    [InlineData("missing-domain@")]
    [InlineData("no-dot@domain")]
    public void ChangeEmail_RejectsInvalidEmailFormat(string email)
    {
        var supplier = Supplier.Create("Acme Supplies");

        Assert.Throws<DomainException>(() => supplier.ChangeEmail(email));
    }
}
