namespace WarehouseERP.Application.ProductCatalog.Products.Commands.CreateProduct;

public sealed class CreateProductCommand
{
    public required string Sku { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required Guid CategoryId { get; init; }
    public required decimal UnitPrice { get; init; }
}
