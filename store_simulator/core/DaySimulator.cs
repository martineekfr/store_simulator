using System;
using store_simulator.models;
using store_simulator.services;

namespace store_simulator.core;

public class DaySimulator
{
    private readonly Store _store;
    private readonly CustomerService _customerService;
    private readonly FinanceService _financeService;
    private Dictionary<string, int> _soldToday = new();


    public DaySimulator(Store store, CustomerService cs, FinanceService fs)
    {
        _store = store;
        _customerService = cs;
        _financeService = fs;
    }

    public void RunDay()
    {
        var customers = _customerService.GenerateCustomers();
        int total = 0;
        foreach (var c in customers)
        {
            var soldProduct = c.Buy(_store);

            if (soldProduct != null)
            {
                if (!_soldToday.ContainsKey(soldProduct.Name))
                    _soldToday[soldProduct.Name] = 0;

                _soldToday[soldProduct.Name]++;
                total += soldProduct.Price;
            }
        }


        var soldToday = new Dictionary<string, int>();

        foreach (var c in customers)
        {
            var sold = c.Buy(_store);

            if (sold != null)
            {
                total += sold.Price;

                if (!soldToday.ContainsKey(sold.Name))
                    soldToday[sold.Name] = 0;

                soldToday[sold.Name]++;
            }
        }

        Console.WriteLine("\n=== Výsledky dne ===");

        if (soldToday.Count == 0)
        {
            Console.WriteLine("Dnes se nic neprodalo.");
        }
        else
        {
            foreach (var kv in soldToday)
                Console.WriteLine($"- {kv.Key} x{kv.Value}");
        }

        Console.WriteLine($"Celkem vyděláno: {total} Kč");
    }
}