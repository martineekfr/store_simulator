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

    public void OrderProducts()
    {
        Console.Write("Název: ");
        var name = Console.ReadLine();

        Console.Write("Cena: ");
        var price = int.Parse(Console.ReadLine()!);

        Console.Write("Množství: ");
        var qty = int.Parse(Console.ReadLine()!);

        var cost = price * qty;

        if (_store.Balance < cost)
        {
            Console.WriteLine("Nedostatek peněz.");
            return;
        }

        var existing = _store.Inventory.FirstOrDefault(i => i.Product.Name == name);

        if (existing == null)
            _store.AddStock(new Product(name!, price), qty);
        else
            existing.Quantity += qty;

        Console.WriteLine("Objednáno.");
    }
}