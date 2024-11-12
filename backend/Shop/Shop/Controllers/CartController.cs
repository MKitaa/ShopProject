using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.DTOs.Cart;
using Shop.Services;

namespace Shop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController(CartService cartService, UserManager<IdentityUser> userManager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCartItems()
    {
        var userId = userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }

        var cartItems = await cartService.GetCartItemsAsync(userId);
        return Ok(cartItems);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
    {
        var userId = userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }

        await cartService.AddToCartAsync(userId, dto.ProductId, dto.Quantity);
        return Ok();
    }

    [HttpPut("update/{cartItemId}")]
    public async Task<IActionResult> UpdateCartItem([FromRoute] int cartItemId, [FromBody] int quantity)
    {
        var userId = userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }

        await cartService.UpdateCartItemAsync(userId, cartItemId, quantity);
        return Ok();
    }

    [HttpDelete("delete/{cartItemId}")]
    public async Task<IActionResult> RemoveFromCart([FromRoute] int cartItemId)
    {
        var userId = userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not authenticated.");
        }

        await cartService.RemoveFromCartAsync(userId, cartItemId);
        return Ok();
    }

    [HttpGet("totalPrice/{cartId}")]
    public async Task<IActionResult> CalculateTotalCartPrice([FromRoute] int cartId)
    {
        var totalPrice = await cartService.CalculateTotalCartPriceAsync(cartId);
        return Ok(totalPrice);
    }
}
