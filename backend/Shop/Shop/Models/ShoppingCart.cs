using Microsoft.AspNetCore.Identity;

namespace Shop.Models;

public class ShoppingCart
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
}
