# Store Simulator

Store Simulator je konzolová aplikace simulující provoz malého obchodu.  
Uživatel může spravovat inventář, objednávat zboží, simulovat denní provoz a sledovat zisky.  
Projekt je navržen tak, aby byl přehledný, rozšiřitelný a dobře strukturovaný podle principů OOP.

---

## Průběžná práce na projektu

Na projektu pracuji postupně a průběžně, což je vidět i v historii commitů na GitHubu.  
Práce probíhala v několika fázích:

### **1) Návrh architektury**
- Rozdělení projektu do složek `models`, `services`, `core`
- Návrh tříd a jejich odpovědností
- Návrh základní logiky obchodu

### **2) Implementace modelů**
- Vytvoření tříd `Product`, `InventoryItem`, `Store`

### **3) Implementace služeb**
- `CustomerService` – generování zákazníků
- `InventoryService` – správa inventáře a objednávek
- `FinanceService` – zpracování denních tržeb

### **4) Herní logika**
- `Game` – hlavní herní smyčka a menu
- `DaySimulator` – simulace jednoho dne v obchodě

### **5) Opravy chyb a stabilizace**
- Oprava namespace
- Oprava chyb kompilace
- Přechod na .NET 8 kvůli chybě CS0579
- Sjednocení struktury projektu
- Testování funkčnosti

---

##  Funkce aplikace

###  Simulace dne
- Náhodně se vygenerují zákazníci
- Každý zákazník nakoupí podle svého typu
- Výsledek dne se přičte k financím obchodu

### Správa inventáře
- Zobrazení inventáře
- Objednání nového zboží
- Kontrola dostupného zůstatku

---

##  Spuštění projektu

V kořenové složce projektu spusť:


---

##  Co je hotové

- Kompletní architektura projektu  
- Všechny modely a služby  
- Herní logika a simulace dne  
- Správa inventáře  
- Objednávky zboží  
- Zákaznické chování  
- Oprava chyb a stabilizace  
- Funkční konzolová aplikace  

---

##  Co ještě zbývá dokončit

###  Možná rozšíření
- Náklady na provoz (energie, nájem…)
- Reklama a marketing (zvyšování počtu zákazníků)
- Statistiky a grafy (vývoj zisku)

### Typy zákazníků
- **CheapCustomer** – kupuje nejlevnější produkt  
- **ImpulsiveCustomer** – kupuje náhodný produkt  
- **DemandingCustomer** – kupuje nejdražší produkt  

---

##  Závěr

Projekt je funkční, přehledný a připravený k dalšímu rozšiřování.  
V této fázi je vhodný pro kontrolní bod a připravený na zpětnou vazbu.



