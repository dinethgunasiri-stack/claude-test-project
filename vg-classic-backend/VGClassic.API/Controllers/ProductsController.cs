using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VGClassic.Application.Products.Commands.CreateProduct;
using VGClassic.Application.Products.Commands.DeleteProduct;
using VGClassic.Application.Products.Commands.UpdateProduct;
using VGClassic.Application.Products.Queries.GetProductById;
using VGClassic.Application.Products.Queries.GetProducts;

namespace VGClassic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProduct(int id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> CreateProduct(CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetProduct), new { id = result.Value }, result.Value)
            : BadRequest(result.Errors);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);
        return result.IsSuccess ? NoContent() : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _mediator.Send(new DeleteProductCommand { Id = id });
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }
}
