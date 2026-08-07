using Microsoft.AspNetCore.Mvc;
using WarehouseERP.Api.Contracts.Products;
using WarehouseERP.Application.Common;
using WarehouseERP.Application.ProductCatalog.Products.Commands.ActivateProduct;
using WarehouseERP.Application.ProductCatalog.Products.Commands.CreateProduct;
using WarehouseERP.Application.ProductCatalog.Products.Commands.DeactivateProduct;
using WarehouseERP.Application.ProductCatalog.Products.Commands.UpdateProduct;
using WarehouseERP.Application.ProductCatalog.Products.Queries.GetProductById;
using WarehouseERP.Application.ProductCatalog.Products.Queries.GetProducts;
using WarehouseERP.Shared.Contracts.Products;
using ApplicationProductDto = WarehouseERP.Application.ProductCatalog.Products.ProductDto;

namespace WarehouseERP.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly IQueryHandler<GetProductsQuery, IReadOnlyList<ApplicationProductDto>> _getProducts;
    private readonly IQueryHandler<GetProductByIdQuery, ApplicationProductDto> _getProductById;
    private readonly ICommandHandler<CreateProductCommand, ApplicationProductDto> _createProduct;
    private readonly ICommandHandler<UpdateProductCommand, ApplicationProductDto> _updateProduct;
    private readonly ICommandHandler<ActivateProductCommand, ApplicationProductDto> _activateProduct;
    private readonly ICommandHandler<DeactivateProductCommand, ApplicationProductDto> _deactivateProduct;

    public ProductsController(
        IQueryHandler<GetProductsQuery, IReadOnlyList<ApplicationProductDto>> getProducts,
        IQueryHandler<GetProductByIdQuery, ApplicationProductDto> getProductById,
        ICommandHandler<CreateProductCommand, ApplicationProductDto> createProduct,
        ICommandHandler<UpdateProductCommand, ApplicationProductDto> updateProduct,
        ICommandHandler<ActivateProductCommand, ApplicationProductDto> activateProduct,
        ICommandHandler<DeactivateProductCommand, ApplicationProductDto> deactivateProduct)
    {
        _getProducts = getProducts;
        _getProductById = getProductById;
        _createProduct = createProduct;
        _updateProduct = updateProduct;
        _activateProduct = activateProduct;
        _deactivateProduct = deactivateProduct;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _getProducts.HandleAsync(new GetProductsQuery(), cancellationToken);

        return Ok(products.ToContract());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await _getProductById.HandleAsync(new GetProductByIdQuery { Id = id }, cancellationToken);

        return Ok(product.ToContract());
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand
        {
            Sku = request.Sku,
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
            UnitPrice = request.UnitPrice
        };

        var product = await _createProduct.HandleAsync(command, cancellationToken);
        var contract = product.ToContract();

        return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductDto>> Update(Guid id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand
        {
            Id = id,
            Sku = request.Sku,
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
            UnitPrice = request.UnitPrice
        };

        var product = await _updateProduct.HandleAsync(command, cancellationToken);

        return Ok(product.ToContract());
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<ActionResult<ProductDto>> Activate(Guid id, CancellationToken cancellationToken)
    {
        var product = await _activateProduct.HandleAsync(new ActivateProductCommand { Id = id }, cancellationToken);

        return Ok(product.ToContract());
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<ActionResult<ProductDto>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var product = await _deactivateProduct.HandleAsync(new DeactivateProductCommand { Id = id }, cancellationToken);

        return Ok(product.ToContract());
    }
}
