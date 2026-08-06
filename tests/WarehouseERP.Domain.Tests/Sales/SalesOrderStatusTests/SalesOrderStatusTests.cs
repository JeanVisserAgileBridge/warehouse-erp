using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.SalesOrderStatusTests;

public class SalesOrderStatusTests
{
    [Theory]
    [InlineData(SalesOrderStatus.Draft)]
    [InlineData(SalesOrderStatus.Confirmed)]
    [InlineData(SalesOrderStatus.PartiallyFulfilled)]
    [InlineData(SalesOrderStatus.Fulfilled)]
    [InlineData(SalesOrderStatus.Cancelled)]
    public void SalesOrderStatus_DefinesExpectedValue(SalesOrderStatus status)
    {
        Assert.True(Enum.IsDefined(status));
    }

    [Fact]
    public void SalesOrderStatus_DefinesExactlyFiveValues()
    {
        var values = Enum.GetValues<SalesOrderStatus>();

        Assert.Equal(5, values.Length);
    }
}
