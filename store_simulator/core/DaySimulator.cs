using System;
using store_simulator.models;
using store_simulator.services;

namespace store_simulator.core;

public class DaySimulator
{
    private readonly Store _store;
    private readonly CustomerService _customerService;
    private readonly FinanceService _financeService;

    public DaySimulator(Store store, CustomerService cs, FinanceService fs)
    {
        _store = store;
        _customerService = cs;
        _financeService = fs;
    }

    public void RunDay()
    {
        var customers = _customerService.GenerateCustomers();
        var total = 0;

        foreach (var c in customers)
            total += c.Buy(_store);

        _financeService.ProcessDay(total);

        Console.WriteLine($"Zisk dne: {total}");
    }
}