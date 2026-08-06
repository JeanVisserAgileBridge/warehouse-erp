using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.PurchaseOrderStatusTests;

public class PurchaseOrderStatusTests
{
    [Theory]
    [InlineData(PurchaseOrderStatus.Draft)]
    [InlineData(PurchaseOrderStatus.Submitted)]
    [InlineData(PurchaseOrderStatus.PartiallyReceived)]
    [InlineData(PurchaseOrderStatus.Received)]
    [InlineData(PurchaseOrderStatus.Cancelled)]
    public void PurchaseOrderStatus_DefinesExpectedValue(PurchaseOrderStatus status)
    {
        Assert.True(Enum.IsDefined(status));
    }

    [Fact]
    public void PurchaseOrderStatus_DefinesExactlyFiveValues()
    {
        var values = Enum.GetValues<PurchaseOrderStatus>();

        Assert.Equal(5, values.Length);
    }
}
