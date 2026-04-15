using System;
using System.Collections.Generic;
using store_simulator.models;

namespace store_simulator.services;

public class CustomerService
{
    private static readonly Func<Customer>[] CustomerFactories =
    [
        static () => new CheapCustomer(),
        static () => new ImpulsiveCustomer(),
        static () => new DemandingCustomer()
    ];

    private readonly Random _random = new();

    public List<Customer> GenerateCustomers()
    {
        var count = _random.Next(3, 8);
        var list = new List<Customer>(count);

        for (var i = 0; i < count; i++)
            list.Add(CustomerFactories[_random.Next(CustomerFactories.Length)]());

        return list;
    }
}
