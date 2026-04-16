using System;
using store_simulator.models;
using store_simulator.services;

namespace store_simulator.core;

public class Game
{
    private readonly Store _store;
    private readonly CustomerService _customerService;
    private readonly FinanceService _financeService;
    private readonly InventoryService _inventoryService;

    public Game(Store store, CustomerService cs, FinanceService fs, InventoryService inv)
    {
        _store = store;
        _customerService = cs;
        _financeService = fs;
        _inventoryService = inv;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\n1) Simulovat den");
            Console.WriteLine("2) Inventář");
            Console.WriteLine("3) Objednat");
            Console.WriteLine("4) Konec");

            var input = Console.ReadLine();

            if (input == "1")
                new DaySimulator(_store, _customerService, _financeService).RunDay();
            else if (input == "2")
                _inventoryService.ShowInventory();
            else if (input == "3")
                _inventoryService.OrderProduct();
            else if (input == "4")
                return;
        }
    }
}