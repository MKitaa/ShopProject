namespace Shop.Models;

public class ShoppingCartItem
{
    public int Id { get; set; }
    public ShoppingCart ShoppingCart { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; }
}