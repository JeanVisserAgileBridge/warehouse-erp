using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.SupplierTests;

public class SupplierCreateTests
{
    [Fact]
    public void Create_ReturnsSupplierWithNonEmptyGuid()
    {
        var supplier = Supplier.Create("Acme Supplies");

        Assert.NotEqual(Guid.Empty, supplier.Id);
    }

    [Fact]
    public void Create_StoresSuppliedValues()
    {
        var supplier = Supplier.Create("Acme Supplies", "contact@acme.com", "555-0100", "1 Industrial Way");

        Assert.Equal("Acme Supplies", supplier.Name);
        Assert.Equal("contact@acme.com", supplier.Email);
        Assert.Equal("555-0100", supplier.PhoneNumber);
        Assert.Equal("1 Industrial Way", supplier.Address);
    }

    [Fact]
    public void Create_MakesSupplierActive()
    {
        var supplier = Supplier.Create("Acme Supplies");

        Assert.True(supplier.IsActive);
    }

    [Fact]
    public void Create_SetsCreatedAtAndUpdatedAtToSameValue()
    {
        var supplier = Supplier.Create("Acme Supplies");

        Assert.Equal(supplier.CreatedAt, supplier.UpdatedAt);
    }

    [Fact]
    public void Create_AcceptsNullEmailPhoneNumberAndAddress()
    {
        var supplier = Supplier.Create("Acme Supplies");

        Assert.Null(supplier.Email);
        Assert.Null(supplier.PhoneNumber);
        Assert.Null(supplier.Address);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_RejectsNullEmptyOrWhitespaceName(string? name)
    {
        Assert.Throws<DomainException>(() => Supplier.Create(name!));
    }

    [Fact]
    public void Create_RejectsNameLongerThanMaxLength()
    {
        var name = new string('a', Supplier.MaxNameLength + 1);

        Assert.Throws<DomainException>(() => Supplier.Create(name));
    }

    [Fact]
    public void Create_AcceptsNameAtMaxLength()
    {
        var name = new string('a', Supplier.MaxNameLength);

        var supplier = Supplier.Create(name);

        Assert.Equal(name, supplier.Name);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("missing-at-sign.com")]
    [InlineData("@missing-local.com")]
    [InlineData("missing-domain@")]
    [InlineData("no-dot@domain")]
    public void Create_RejectsInvalidEmailFormat(string email)
    {
        Assert.Throws<DomainException>(() => Supplier.Create("Acme Supplies", email));
    }

    [Fact]
    public void Create_AcceptsValidEmail()
    {
        var supplier = Supplier.Create("Acme Supplies", "contact@acme.com");

        Assert.Equal("contact@acme.com", supplier.Email);
    }

    [Fact]
    public void Create_RejectsPhoneNumberLongerThanMaxLength()
    {
        var phoneNumber = new string('1', Supplier.MaxPhoneNumberLength + 1);

        Assert.Throws<DomainException>(() => Supplier.Create("Acme Supplies", phoneNumber: phoneNumber));
    }

    [Fact]
    public void Create_AcceptsPhoneNumberAtMaxLength()
    {
        var phoneNumber = new string('1', Supplier.MaxPhoneNumberLength);

        var supplier = Supplier.Create("Acme Supplies", phoneNumber: phoneNumber);

        Assert.Equal(phoneNumber, supplier.PhoneNumber);
    }

    [Fact]
    public void Create_RejectsAddressLongerThanMaxLength()
    {
        var address = new string('a', Supplier.MaxAddressLength + 1);

        Assert.Throws<DomainException>(() => Supplier.Create("Acme Supplies", address: address));
    }

    [Fact]
    public void Create_AcceptsAddressAtMaxLength()
    {
        var address = new string('a', Supplier.MaxAddressLength);

        var supplier = Supplier.Create("Acme Supplies", address: address);

        Assert.Equal(address, supplier.Address);
    }
}
