namespace WarehouseERP.Application.ProductCatalog.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommand
{
    public required Guid Id { get; init; }
    public required string Sku { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required Guid CategoryId { get; init; }
    public required decimal UnitPrice { get; init; }
}
