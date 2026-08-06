using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.CustomerTests;

public class CustomerCreateTests
{
    [Fact]
    public void Create_ReturnsCustomerWithNonEmptyGuid()
    {
        var customer = Customer.Create("Jane Doe");

        Assert.NotEqual(Guid.Empty, customer.Id);
    }

    [Fact]
    public void Create_StoresSuppliedValues()
    {
        var customer = Customer.Create("Jane Doe", "jane@example.com", "555-0100", "1 Main Street");

        Assert.Equal("Jane Doe", customer.Name);
        Assert.Equal("jane@example.com", customer.Email);
        Assert.Equal("555-0100", customer.PhoneNumber);
        Assert.Equal("1 Main Street", customer.Address);
    }

    [Fact]
    public void Create_MakesCustomerActive()
    {
        var customer = Customer.Create("Jane Doe");

        Assert.True(customer.IsActive);
    }

    [Fact]
    public void Create_SetsCreatedAtAndUpdatedAtToSameValue()
    {
        var customer = Customer.Create("Jane Doe");

        Assert.Equal(customer.CreatedAt, customer.UpdatedAt);
    }

    [Fact]
    public void Create_AcceptsNullEmailPhoneNumberAndAddress()
    {
        var customer = Customer.Create("Jane Doe");

        Assert.Null(customer.Email);
        Assert.Null(customer.PhoneNumber);
        Assert.Null(customer.Address);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_RejectsNullEmptyOrWhitespaceName(string? name)
    {
        Assert.Throws<DomainException>(() => Customer.Create(name!));
    }

    [Fact]
    public void Create_RejectsNameLongerThanMaxLength()
    {
        var name = new string('a', Customer.MaxNameLength + 1);

        Assert.Throws<DomainException>(() => Customer.Create(name));
    }

    [Fact]
    public void Create_AcceptsNameAtMaxLength()
    {
        var name = new string('a', Customer.MaxNameLength);

        var customer = Customer.Create(name);

        Assert.Equal(name, customer.Name);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("missing-at-sign.com")]
    [InlineData("@missing-local.com")]
    [InlineData("missing-domain@")]
    [InlineData("no-dot@domain")]
    public void Create_RejectsInvalidEmailFormat(string email)
    {
        Assert.Throws<DomainException>(() => Customer.Create("Jane Doe", email));
    }

    [Fact]
    public void Create_AcceptsValidEmail()
    {
        var customer = Customer.Create("Jane Doe", "jane@example.com");

        Assert.Equal("jane@example.com", customer.Email);
    }

    [Fact]
    public void Create_RejectsPhoneNumberLongerThanMaxLength()
    {
        var phoneNumber = new string('1', Customer.MaxPhoneNumberLength + 1);

        Assert.Throws<DomainException>(() => Customer.Create("Jane Doe", phoneNumber: phoneNumber));
    }

    [Fact]
    public void Create_AcceptsPhoneNumberAtMaxLength()
    {
        var phoneNumber = new string('1', Customer.MaxPhoneNumberLength);

        var customer = Customer.Create("Jane Doe", phoneNumber: phoneNumber);

        Assert.Equal(phoneNumber, customer.PhoneNumber);
    }

    [Fact]
    public void Create_RejectsAddressLongerThanMaxLength()
    {
        var address = new string('a', Customer.MaxAddressLength + 1);

        Assert.Throws<DomainException>(() => Customer.Create("Jane Doe", address: address));
    }

    [Fact]
    public void Create_AcceptsAddressAtMaxLength()
    {
        var address = new string('a', Customer.MaxAddressLength);

        var customer = Customer.Create("Jane Doe", address: address);

        Assert.Equal(address, customer.Address);
    }
}
