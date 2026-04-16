using System;
using System.Linq;
using store_simulator.models;

namespace store_simulator.services;

public class InventoryService
{
    private readonly Store _store;

    public InventoryService(Store store)
    {
        _store = store;
    }

    public void ShowInventory()
    {
        Console.WriteLine("=== Inventář ===");

        foreach (var item in _store.Inventory)
            Console.WriteLine($"{item.Product.Name} | {item.Product.Price} | {item.Quantity}");
            Console.WriteLine($"Peněženka: {_store.Balance} Kč\n");
    }

    public void OrderProduct()
    {
        Console.Write("Název produktu: ");
        var name = Console.ReadLine();

        Console.Write("Cena: ");
        var price = int.Parse(Console.ReadLine());

        Console.Write("Množství: ");
        var quantity = int.Parse(Console.ReadLine());

        int totalCost = price * quantity;

        if (_store.Balance < totalCost)
        {
            Console.WriteLine($"Nemáš dost peněz! Objednávka stojí {totalCost} Kč, ale máš jen {_store.Balance} Kč.");
            return;
        }

        _store.Balance -= totalCost;   
        var existing = _store.Inventory.FirstOrDefault(i => i.Product.Name == name);

        if (existing == null)
            _store.AddStock(new Product(name!, price), quantity);
        else
            existing.Quantity += quantity;

        Console.WriteLine("Objednáno.");

    }

}