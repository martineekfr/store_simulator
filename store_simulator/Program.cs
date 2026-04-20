using store_simulator.core;
using store_simulator.models;
using store_simulator.services;

namespace store_simulator;

internal class Program
{
    static void Main(string[] args)
    {
        var store = new Store(1000);
        var cs = new CustomerService();
        var fs = new FinanceService(store);
        var inv = new InventoryService(store);

        var game = new Game(store, cs, fs, inv);
        Console.WriteLine("__STORE_SIMULATOR__");
        game.Run();
    }
}