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
            Console.WriteLine($"{item.Product.Name} | {item.Product.Price} Kč | {item.Quantity} celkem" );
            Console.WriteLine($"Peněženka: {_store.Balance} Kč\n");
    }

    public void OrderProduct()
    {
        Console.WriteLine("Výběr produktů:\n" +
                          "Mrkev 20 Kč\n" +
                          "Brambory 50kč\n" +
                          "Bonbony 59kč");
        Console.Write("\nNázev produktu: ");
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

        Console.WriteLine($"Objednáno. Bylo odečteno -{totalCost} Kč z tvého účtu, nyní máš na účtu {_store.Balance} Kč." );

    }

}