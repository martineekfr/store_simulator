namespace store_simulator.models;

public class InventoryItem
{
    public Product Product { get; }
    public int Quantity { get; set; }

    public InventoryItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }
}