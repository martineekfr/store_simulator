using System;
using System.Collections.Generic;
using System.Linq;

namespace store_simulator.models;

public class Store
{
    private readonly Random _random = new();

    public int Balance { get; private set; }
    public List<InventoryItem> Inventory { get; } = new();

    public Store(int startingBalance)
    {
        Balance = startingBalance;
    }

    public int SellCheapestProduct()
    {
        InventoryItem? cheapest = null;

        foreach (var item in Inventory)
        {
            if (item.Quantity <= 0)
                continue;

            if (cheapest == null || item.Product.Price < cheapest.Product.Price)
                cheapest = item;
        }

        return SellItem(cheapest);
    }

    public int SellMostExpensiveProduct()
    {
        InventoryItem? mostExpensive = null;

        foreach (var item in Inventory)
        {
            if (item.Quantity <= 0)
                continue;

            if (mostExpensive == null || item.Product.Price > mostExpensive.Product.Price)
                mostExpensive = item;
        }

        return SellItem(mostExpensive);
    }

    public int SellRandomProduct()
    {
        InventoryItem? selected = null;
        var seenItems = 0;

        foreach (var item in Inventory)
        {
            if (item.Quantity <= 0)
                continue;

            seenItems++;

            // Reservoir sampling avoids allocating a temporary filtered list.
            if (_random.Next(seenItems) == 0)
                selected = item;
        }

        return SellItem(selected);
    }

    public void AddStock(Product product, int quantity)
    {
        if (quantity <= 0)
            return;

        var existing = Inventory.FirstOrDefault(
            i => string.Equals(i.Product.Name, product.Name, StringComparison.OrdinalIgnoreCase));

        if (existing == null)
            Inventory.Add(new InventoryItem(product, quantity));
        else
            existing.Quantity += quantity;
    }

    public void AddRevenue(int amount)
    {
        if (amount <= 0)
            return;

        Balance += amount;
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0)
            return true;

        if (Balance < amount)
            return false;

        Balance -= amount;
        return true;
    }

    private static int SellItem(InventoryItem? item)
    {
        if (item == null)
            return 0;

        item.Quantity--;
        return item.Product.Price;
    }
}
