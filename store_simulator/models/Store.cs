using System;
using System.Collections.Generic;
using System.Linq;

namespace store_simulator.models;

public class Store
{
    private readonly Random _random = new();

    public int Balance { get; set; }
    public List<InventoryItem> Inventory { get; } = new();

    public Store(int startingBalance)
    {
        Balance = startingBalance;
    }

    public int SellCheapestProduct()
    {
        var item = Inventory.Where(i => i.Quantity > 0)
            .OrderBy(i => i.Product.Price)
            .FirstOrDefault();

        return SellItem(item);
    }

    public int SellMostExpensiveProduct()
    {
        var item = Inventory.Where(i => i.Quantity > 0)
            .OrderByDescending(i => i.Product.Price)
            .FirstOrDefault();

        return SellItem(item);
    }

    public int SellRandomProduct()
    {
        var available = Inventory.Where(i => i.Quantity > 0).ToList();
        if (available.Count == 0) return 0;

        var item = available[_random.Next(available.Count)];
        return SellItem(item);
    }

    public void AddStock(Product product, int quantity)
    {
        var existing = Inventory.FirstOrDefault(i => i.Product.Name == product.Name);

        if (existing == null)
            Inventory.Add(new InventoryItem(product, quantity));
        else
            existing.Quantity += quantity;
    }

    public void AddRevenue(int amount)
    {
        Balance += amount;
    }

    private int SellItem(InventoryItem? item)
    {
        if (item == null) return 0;

        item.Quantity--;
        Balance += item.Product.Price;
        return item.Product.Price;
    }
}