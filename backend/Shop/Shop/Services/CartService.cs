using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.DTOs.Cart;
using Shop.Models;

namespace Shop.Services;

public class CartService(ShopDbContext context, IMapper mapper)
{
    private async Task<ShoppingCart> GetCartAsync(string userId)
    {
        var cart = await context.ShoppingCarts
            .Include(c => c.ShoppingCartItems)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart != null) return cart;

        cart = new ShoppingCart { UserId = userId, CreatedAt = DateTime.UtcNow };
        await context.ShoppingCarts.AddAsync(cart);
        await context.SaveChangesAsync();

        return cart;
    }

    public async Task AddToCartAsync(string userId, int productId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        var cart = await GetCartAsync(userId);

        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null) throw new ArgumentException("Produkt nie istnieje.");

        var cartItem = cart.ShoppingCartItems.FirstOrDefault(item => item.ProductId == productId);

        if (cartItem != null)
            cartItem.Quantity++;
        else
        {
            cart.ShoppingCartItems.Add(new ShoppingCartItem
            {
                ProductId = productId,
                Quantity = quantity
            });
        }

        await context.SaveChangesAsync();
    }

    public async Task UpdateCartItemAsync(string userId, int cartItemId, int quantity)
    {
        var cartItem = context.ShoppingCartItems.Include(shoppingCartItem => shoppingCartItem.ShoppingCart)
            .FirstOrDefault(i => i.ProductId == cartItemId);
        if (cartItem == null || cartItem.ShoppingCart.UserId != userId)
        {
            throw new ArgumentException("Produkt nie istnieje w koszyku.");
        }

        Console.WriteLine(quantity);
        cartItem.Quantity = quantity;
        await context.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(string userId, int cartItemId)
    {
        var cartItem = context.ShoppingCartItems.Include(shoppingCartItem => shoppingCartItem.ShoppingCart)
            .FirstOrDefault(i => i.ProductId == cartItemId);
        Console.WriteLine(cartItem);
        Console.WriteLine(cartItem.ShoppingCart.UserId);
        Console.WriteLine(userId);
        if (cartItem == null || cartItem.ShoppingCart.UserId != userId)
        {
            throw new ArgumentException("Produkt nie istnieje w koszyku.");
        }

        context.ShoppingCartItems.Remove(cartItem);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ShoppingCartDto>> GetCartItemsAsync(string userId)
    {
        var cart = await GetCartAsync(userId);
        var cartItemsDto = mapper.Map<IEnumerable<ShoppingCartDto>>(cart.ShoppingCartItems);
        return cartItemsDto;
    }

    public async Task<decimal> CalculateTotalCartPriceAsync(int cartId)
    {
        var totalPrice = await context.Database.ExecuteSqlAsync($"SELECT CalculateTotalCartPrice({cartId})");
        return totalPrice;
    }
}
