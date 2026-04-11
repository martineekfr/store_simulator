namespace store_simulator.models;

public abstract class Customer
{
    public abstract int Buy(Store store);
}

public class CheapCustomer : Customer
{
    public override int Buy(Store store) => store.SellCheapestProduct();
}

public class ImpulsiveCustomer : Customer
{
    public override int Buy(Store store) => store.SellRandomProduct();
}

public class DemandingCustomer : Customer
{
    public override int Buy(Store store) => store.SellMostExpensiveProduct();
}