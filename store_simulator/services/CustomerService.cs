using System;
using System.Collections.Generic;
using store_simulator.models;

namespace store_simulator.services;

public class CustomerService
{
    private readonly Random _random = new();

    public List<Customer> GenerateCustomers()
    {
        var count = _random.Next(3, 8);
        var list = new List<Customer>();

        for (int i = 0; i < count; i++)
        {
            var type = _random.Next(3);
            list.Add(type switch
            {
                0 => new CheapCustomer(),
                1 => new ImpulsiveCustomer(),
                _ => new DemandingCustomer()
            });
        }

        return list;
    }
}