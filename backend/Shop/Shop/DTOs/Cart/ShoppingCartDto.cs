namespace Shop.DTOs.Cart;

public class ShoppingCartDto
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
