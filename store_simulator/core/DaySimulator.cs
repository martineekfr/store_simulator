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
        var revenue = 0;
        var successfulSales = 0;

        foreach (var customer in customers)
        {
            var purchaseValue = customer.Buy(_store);
            revenue += purchaseValue;

            if (purchaseValue > 0)
                successfulSales++;
        }

        _financeService.ProcessDay(revenue);

        Console.WriteLine(
            $"Dnes prislo {customers.Count} zakazniku, probehlo {successfulSales} nakupu, trzba dne: {revenue}");
    }
}
