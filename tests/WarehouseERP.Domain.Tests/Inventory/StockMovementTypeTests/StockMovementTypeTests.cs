using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Domain.Tests.Inventory.StockMovementTypeTests;

public class StockMovementTypeTests
{
    [Theory]
    [InlineData(StockMovementType.Receipt)]
    [InlineData(StockMovementType.Issue)]
    [InlineData(StockMovementType.Adjustment)]
    [InlineData(StockMovementType.Transfer)]
    [InlineData(StockMovementType.Return)]
    public void StockMovementType_DefinesExpectedValue(StockMovementType movementType)
    {
        Assert.True(Enum.IsDefined(movementType));
    }

    [Fact]
    public void StockMovementType_DefinesExactlyFiveValues()
    {
        var values = Enum.GetValues<StockMovementType>();

        Assert.Equal(5, values.Length);
    }
}
