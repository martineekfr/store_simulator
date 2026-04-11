namespace store_simulator.models;

public class Product
{
    public string Name { get; }
    public int Price { get; }

    public Product(string name, int price)
    {
        Name = name;
        Price = price;
    }
}