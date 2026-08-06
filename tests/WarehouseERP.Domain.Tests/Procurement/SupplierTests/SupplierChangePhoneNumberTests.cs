using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.SupplierTests;

public class SupplierChangePhoneNumberTests
{
    [Fact]
    public void ChangePhoneNumber_UpdatesPhoneNumber()
    {
        var supplier = Supplier.Create("Acme Supplies", phoneNumber: "555-0100");

        supplier.ChangePhoneNumber("555-0199");

        Assert.Equal("555-0199", supplier.PhoneNumber);
    }

    [Fact]
    public void ChangePhoneNumber_ClearsPhoneNumberWhenNull()
    {
        var supplier = Supplier.Create("Acme Supplies", phoneNumber: "555-0100");

        supplier.ChangePhoneNumber(null);

        Assert.Null(supplier.PhoneNumber);
    }

    [Fact]
    public void ChangePhoneNumber_UpdatesUpdatedAt()
    {
        var supplier = Supplier.Create("Acme Supplies");
        var originalUpdatedAt = supplier.UpdatedAt;

        supplier.ChangePhoneNumber("555-0199");

        Assert.True(supplier.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ChangePhoneNumber_RejectsPhoneNumberLongerThanMaxLength()
    {
        var supplier = Supplier.Create("Acme Supplies");
        var phoneNumber = new string('1', Supplier.MaxPhoneNumberLength + 1);

        Assert.Throws<DomainException>(() => supplier.ChangePhoneNumber(phoneNumber));
    }

    [Fact]
    public void ChangePhoneNumber_AcceptsPhoneNumberAtMaxLength()
    {
        var supplier = Supplier.Create("Acme Supplies");
        var phoneNumber = new string('1', Supplier.MaxPhoneNumberLength);

        supplier.ChangePhoneNumber(phoneNumber);

        Assert.Equal(phoneNumber, supplier.PhoneNumber);
    }
}
