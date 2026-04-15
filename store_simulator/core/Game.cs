using System;
using store_simulator.models;
using store_simulator.services;

namespace store_simulator.core;

public class Game
{
    private readonly Store _store;
    private readonly InventoryService _inventoryService;
    private readonly DaySimulator _daySimulator;

    public Game(Store store, CustomerService cs, FinanceService fs, InventoryService inv)
    {
        _store = store;
        _inventoryService = inv;
        _daySimulator = new DaySimulator(store, cs, fs);
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine($"\nStav uctu: {_store.Balance}");
            Console.WriteLine("1) Simulovat den");
            Console.WriteLine("2) Inventar");
            Console.WriteLine("3) Objednat");
            Console.WriteLine("4) Konec");
            Console.Write("Volba: ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    _daySimulator.RunDay();
                    break;
                case "2":
                    _inventoryService.ShowInventory();
                    break;
                case "3":
                    _inventoryService.OrderProducts();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Neplatna volba. Zadej 1 az 4.");
                    break;
            }
        }
    }
}
