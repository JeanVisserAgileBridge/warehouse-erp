using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.CustomerTests;

public class CustomerChangePhoneNumberTests
{
    [Fact]
    public void ChangePhoneNumber_UpdatesPhoneNumber()
    {
        var customer = Customer.Create("Jane Doe", phoneNumber: "555-0100");

        customer.ChangePhoneNumber("555-0199");

        Assert.Equal("555-0199", customer.PhoneNumber);
    }

    [Fact]
    public void ChangePhoneNumber_ClearsPhoneNumberWhenNull()
    {
        var customer = Customer.Create("Jane Doe", phoneNumber: "555-0100");

        customer.ChangePhoneNumber(null);

        Assert.Null(customer.PhoneNumber);
    }

    [Fact]
    public void ChangePhoneNumber_UpdatesUpdatedAt()
    {
        var customer = Customer.Create("Jane Doe");
        var originalUpdatedAt = customer.UpdatedAt;

        customer.ChangePhoneNumber("555-0199");

        Assert.True(customer.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ChangePhoneNumber_RejectsPhoneNumberLongerThanMaxLength()
    {
        var customer = Customer.Create("Jane Doe");
        var phoneNumber = new string('1', Customer.MaxPhoneNumberLength + 1);

        Assert.Throws<DomainException>(() => customer.ChangePhoneNumber(phoneNumber));
    }

    [Fact]
    public void ChangePhoneNumber_AcceptsPhoneNumberAtMaxLength()
    {
        var customer = Customer.Create("Jane Doe");
        var phoneNumber = new string('1', Customer.MaxPhoneNumberLength);

        customer.ChangePhoneNumber(phoneNumber);

        Assert.Equal(phoneNumber, customer.PhoneNumber);
    }
}
