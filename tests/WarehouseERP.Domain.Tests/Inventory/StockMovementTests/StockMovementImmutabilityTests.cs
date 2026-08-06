using System.Reflection;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Domain.Tests.Inventory.StockMovementTests;

public class StockMovementImmutabilityTests
{
    [Fact]
    public void StockMovement_ExposesNoPublicInstanceMethodsOtherThanPropertyAccessors()
    {
        var publicInstanceMethods = typeof(StockMovement)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(method => !method.IsSpecialName);

        Assert.Empty(publicInstanceMethods);
    }

    [Fact]
    public void StockMovement_HasNoPublicPropertySetters()
    {
        var publicSetters = typeof(StockMovement)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(property => property.GetSetMethod(nonPublic: false))
            .Where(setter => setter is not null);

        Assert.Empty(publicSetters);
    }
}
