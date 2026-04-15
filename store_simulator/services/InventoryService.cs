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
        Console.WriteLine("=== Inventar ===");

        if (_store.Inventory.Count == 0 || _store.Inventory.All(i => i.Quantity <= 0))
        {
            Console.WriteLine("Inventar je prazdny.");
            return;
        }

        foreach (var item in _store.Inventory.OrderBy(i => i.Product.Name))
            Console.WriteLine($"{item.Product.Name} | Cena: {item.Product.Price} | Kusy: {item.Quantity}");
    }

    public void OrderProducts()
    {
        var name = ReadRequiredText("Nazev: ");
        var price = ReadPositiveInt("Cena: ");
        var quantity = ReadPositiveInt("Mnozstvi: ");

        int cost;
        try
        {
            cost = checked(price * quantity);
        }
        catch (OverflowException)
        {
            Console.WriteLine("Objednavka je prilis velka.");
            return;
        }

        if (!_store.TrySpend(cost))
        {
            Console.WriteLine("Nedostatek penez.");
            return;
        }

        _store.AddStock(new Product(name, price), quantity);
        Console.WriteLine($"Objednano za {cost}. Aktualni zustatek: {_store.Balance}");
    }

    private static string ReadRequiredText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var value = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();

            Console.WriteLine("Hodnota nesmi byt prazdna.");
        }
    }

    private static int ReadPositiveInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var value = Console.ReadLine();

            if (int.TryParse(value, out var number) && number > 0)
                return number;

            Console.WriteLine("Zadej kladne cele cislo.");
        }
    }
}
