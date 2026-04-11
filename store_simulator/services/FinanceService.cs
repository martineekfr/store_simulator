using store_simulator.models;

namespace store_simulator.services;

public class FinanceService
{
    private readonly Store _store;

    public FinanceService(Store store)
    {
        _store = store;
    }

    public void ProcessDay(int revenue)
    {
        _store.AddRevenue(revenue);
    }
}