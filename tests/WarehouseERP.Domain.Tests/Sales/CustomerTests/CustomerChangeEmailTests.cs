using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.CustomerTests;

public class CustomerChangeEmailTests
{
    [Fact]
    public void ChangeEmail_UpdatesEmail()
    {
        var customer = Customer.Create("Jane Doe", "old@example.com");

        customer.ChangeEmail("new@example.com");

        Assert.Equal("new@example.com", customer.Email);
    }

    [Fact]
    public void ChangeEmail_ClearsEmailWhenNull()
    {
        var customer = Customer.Create("Jane Doe", "old@example.com");

        customer.ChangeEmail(null);

        Assert.Null(customer.Email);
    }

    [Fact]
    public void ChangeEmail_UpdatesUpdatedAt()
    {
        var customer = Customer.Create("Jane Doe");
        var originalUpdatedAt = customer.UpdatedAt;

        customer.ChangeEmail("new@example.com");

        Assert.True(customer.UpdatedAt >= originalUpdatedAt);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("missing-at-sign.com")]
    [InlineData("@missing-local.com")]
    [InlineData("missing-domain@")]
    [InlineData("no-dot@domain")]
    public void ChangeEmail_RejectsInvalidEmailFormat(string email)
    {
        var customer = Customer.Create("Jane Doe");

        Assert.Throws<DomainException>(() => customer.ChangeEmail(email));
    }
}
