namespace store_simulator.models;

public abstract class Customer
{
    public abstract Product? Buy(Store store);
}

public class CheapCustomer : Customer
{
    public override Product? Buy(Store store)
    {
        return store.SellCheapestProduct();
    }

}

public class ImpulsiveCustomer : Customer
{
    public override Product? Buy(Store store)
    {
        return store.SellRandomProduct();
    }

}

public class DemandingCustomer : Customer
{
    public override Product? Buy(Store store)
    {
        return store.SellMostExpensiveProduct();
    }

}



