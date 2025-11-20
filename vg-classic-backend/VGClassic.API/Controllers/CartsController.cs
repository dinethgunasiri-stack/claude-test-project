using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VGClassic.Application.Carts.Commands.AddToCart;
using VGClassic.Application.Carts.Commands.RemoveFromCart;
using VGClassic.Application.Carts.Queries.GetCart;

namespace VGClassic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var result = await _mediator.Send(new GetCartQuery());
        return Ok(result.Value);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddToCart(AddToCartCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpDelete("items/{itemId}")]
    public async Task<IActionResult> RemoveFromCart(int itemId)
    {
        var result = await _mediator.Send(new RemoveFromCartCommand { ItemId = itemId });
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }
}
