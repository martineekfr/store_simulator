# Store Simulator

Store Simulator je jednoduchá konzolová aplikace simulující provoz malého obchodu.  
Uživatel může spravovat inventář, objednávat zboží, simulovat denní provoz a sledovat zisky.

Projekt je rozdělen do tří hlavních částí:
- **models** – datové struktury a logika obchodu
- **services** – služby pro generování zákazníků, správu inventáře a finance
- **core** – herní logika a simulace dne

---

## 📁 Struktura projektu

store_simulator/
│   Program.cs
│   README.md
│
├── core/
│   ├── Game.cs
│   └── DaySimulator.cs
│
├── models/
│   ├── Store.cs
│   ├── Product.cs
│   ├── InventoryItem.cs
│   └── Customer.cs
│
└── services/
├── CustomerService.cs
├── InventoryService.cs
└── FinanceService.cs


---

## 🛒 Funkce aplikace

### ✔ Simulace dne
- Náhodně se vygenerují zákazníci různých typů
- Každý zákazník nakoupí podle svého chování
- Výsledek dne se přičte k financím obchodu

### ✔ Správa inventáře
- Zobrazení aktuálního inventáře
- Objednání nového zboží
- Kontrola dostupného zůstatku

### ✔ Typy zákazníků
- **CheapCustomer** – kupuje nejlevnější produkt  
- **ImpulsiveCustomer** – kupuje náhodný produkt  
- **DemandingCustomer** – kupuje nejdražší produkt  

---

## ▶️ Spuštění projektu

1. Otevři terminál ve složce projektu
2. Spusť:

dotnet run
