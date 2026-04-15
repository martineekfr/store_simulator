using store_simulator.core;
using store_simulator.models;
using store_simulator.services;

var store = new Store(1000);
var customerService = new CustomerService();
var financeService = new FinanceService(store);
var inventoryService = new InventoryService(store);

var game = new Game(store, customerService, financeService, inventoryService);
game.Run();
