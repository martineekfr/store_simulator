# Detailed Fix Report
## 1. Purpose of this document
This file explains exactly what was fixed, why it was broken, and how each fix was verified.
The goal is to provide traceable engineering detail, not just a short summary.
Every major change is described from symptom to root cause to patch outcome.
The focus is on build stability, runtime correctness, and practical optimization.
This report reflects the state of the code after the stabilization pass.
## 2. Initial state before fixes
The repository had two runnable project entry points in the root and nested folders.
The main game logic lived under `store_simulator/`.
The root project still had a template-style setup and a default `Program.cs` originally.
Generated `obj` and `bin` artifacts were already present in the working tree.
The app could compile through the solution path in some cases.
However, the root build path could fail with duplicate assembly metadata.
Runtime behavior was also inconsistent for money and inventory flows.
Input handling had crash paths for invalid user input.
The game loop did not always guide the user clearly on invalid menu choices.
Performance was acceptable for tiny data but had avoidable allocations.
## 3. Fix group A: build and compilation structure
### 3.1 Symptom
Running `dotnet build ConsoleApp1.csproj` produced `CS0579` duplicate assembly errors.
The errors referenced duplicate assembly attributes and target framework attributes.
### 3.2 Root cause
The root project was using default compile item inclusion.
That behavior can include source files recursively from nested directories.
Because the nested project also generated assembly metadata, the root build pulled conflicting inputs.
This caused duplicate attribute generation and compilation failure.
### 3.3 Code changes
`ConsoleApp1.csproj` now sets `<EnableDefaultCompileItems>false</EnableDefaultCompileItems>`.
A precise compile include was added for only `Program.cs` in the root project.
A `ProjectReference` to `store_simulator/store_simulator.csproj` was added.
This keeps root project ownership clean while still depending on game code.
### 3.4 Why this is correct
The root project now compiles only what it owns.
The nested project compiles and emits its own assembly metadata independently.
There is no longer a path where both metadata sets collide in one compilation unit.
This removes the `CS0579` class of errors for this structure.
### 3.5 Verification
`dotnet build store_simulator.sln` succeeded with zero errors.
`dotnet build ConsoleApp1.csproj` succeeded with zero errors.
The previously failing command now passes consistently.
## 4. Fix group B: root entry point behavior
### 4.1 Symptom
The root `Program.cs` was a template output and did not run the simulator.
This made the repository entry behavior confusing.
### 4.2 Root cause
Project scaffolding remained at root while real app code moved to nested project.
The root launch path did not initialize store services or game loop.
### 4.3 Code changes
Root `Program.cs` now creates `Store`, `CustomerService`, `FinanceService`, and `InventoryService`.
It constructs `Game` and calls `game.Run()`.
This mirrors the expected startup path for the simulator.
### 4.4 Why this is correct
Both root and nested launch paths now align with actual simulator behavior.
Developers can run from root without getting placeholder output.
### 4.5 Verification
`dotnet run` from root opens the simulator menu instead of printing template text.
## 5. Fix group C: double-counting revenue
### 5.1 Symptom
Balance changes were inconsistent and could inflate profit tracking.
A day simulation could apply income more than once depending on flow.
### 5.2 Root cause
`Store.SellItem` was adding product price directly to `Balance`.
`DaySimulator` also summed purchases and passed total to `FinanceService.ProcessDay`.
`FinanceService` then added the same total again through `Store.AddRevenue`.
That means sale value could be applied twice to balance.
### 5.3 Code changes
`Store.SellItem` was changed to only decrement quantity and return sale value.
Revenue application now happens only through daily finance processing.
`Store.AddRevenue` remains the single place that mutates balance for revenue.
### 5.4 Why this is correct
Transaction value should be computed in sale events and posted once to finance.
Separating sale selection from accounting avoids accidental duplicate mutation.
The model now has one clear revenue application point.
### 5.5 Verification
Scripted run sequence confirmed balanced accounting after ordering and day simulation.
Observed numbers matched expected math instead of inflated totals.
## 6. Fix group D: purchase spending not applied
### 6.1 Symptom
Ordering products checked available money but did not reliably deduct purchase cost.
This made balance display misleading and broke economy rules.
### 6.2 Root cause
Order flow computed `cost` and checked `Balance < cost`.
If funds were enough, stock was added, but balance was not decremented in that path.
This allowed free inventory additions.
### 6.3 Code changes
A new method `Store.TrySpend(int amount)` was introduced.
`TrySpend` validates amount, checks balance, and deducts only when valid.
`InventoryService.OrderProducts` now calls `_store.TrySpend(cost)` before adding stock.
Order is rejected cleanly if spending fails.
### 6.4 Why this is correct
Spending logic is centralized and reusable.
Inventory changes are now transactional with cost handling.
The store cannot acquire stock without paying for it.
### 6.5 Verification
Smoke test: order of price 10 and quantity 5 reduced balance by 50.
Follow-up day simulation returned balance to expected value after sales.
## 7. Fix group E: input crash prevention
### 7.1 Symptom
Entering non-numeric price or quantity could throw exceptions and terminate the app.
Empty product names also produced weak behavior.
### 7.2 Root cause
The old code used `int.Parse(Console.ReadLine()!)` without guard rails.
Any non-integer input immediately raised `FormatException`.
Null or empty text was not robustly handled.
### 7.3 Code changes
Added `ReadRequiredText` helper for non-empty product names.
Added `ReadPositiveInt` helper using `int.TryParse` loop.
Prompt now repeats until user enters valid positive integers.
Invalid input no longer tears down the process.
### 7.4 Overflow handling
Cost calculation now uses `checked(price * quantity)`.
Overflow triggers a controlled message instead of silent wraparound.
If overflow occurs, order is canceled safely.
### 7.5 Why this is correct
Console applications must treat input as untrusted.
Validation loops convert hard crashes into guided recovery.
Checked arithmetic protects financial integrity.
### 7.6 Verification
Smoke test with `abc` for price no longer crashed.
The program printed validation feedback and continued normally.
## 8. Fix group F: game loop clarity and resilience
### 8.1 Symptom
Menu flow accepted only known values but gave weak feedback for invalid entries.
The day simulator was instantiated repeatedly inside the loop.
### 8.2 Root cause
Control flow used chained `if/else` with limited invalid branch messaging.
Repeated object creation was unnecessary for stable dependencies.
### 8.3 Code changes
Menu handling now uses `switch` for clearer intent and maintainability.
Invalid choice now prints explicit guidance to enter values 1 to 4.
Current balance is shown each loop iteration.
`DaySimulator` is created once in `Game` constructor and reused.
### 8.4 Why this is correct
Users get immediate actionable feedback.
Object reuse reduces needless allocations and keeps game dependencies stable.
The loop logic is easier to read and maintain.
### 8.5 Verification
Scripted input `9` followed by `4` stayed stable and exited cleanly.
## 9. Fix group G: inventory behavior and display
### 9.1 Symptom
Inventory output was minimal and not predictable in order.
Empty or depleted inventory states were not clearly communicated.
### 9.2 Root cause
Display code simply iterated the raw inventory list.
No empty-state branch existed.
### 9.3 Code changes
`ShowInventory` now checks empty/depleted inventory first.
When empty, it prints an explicit message and returns early.
When non-empty, output is sorted by product name for predictable scanning.
Display labels now include explicit price and quantity wording.
### 9.4 Why this is correct
Predictable ordering improves usability.
Explicit empty-state handling prevents confusion.
### 9.5 Verification
After selling all stock, inventory view reported empty state correctly.
## 10. Fix group H: algorithmic and allocation optimizations
### 10.1 Cheapest and most expensive selection
Old logic filtered and sorted inventory to pick one item.
Sorting is unnecessary when only min or max is needed.
New logic scans once and tracks best candidate.
Complexity changed from sort-driven selection to single-pass selection.
Allocation pressure was reduced by removing temporary LINQ sort structures.
### 10.2 Random selection optimization
Old random sale path built a temporary list of available items.
That allocation occurs every random customer purchase.
New logic uses reservoir sampling during one pass.
No temporary list is created.
Selection remains unbiased among available items.
### 10.3 Customer generation optimization
Customer list now preallocates capacity using known count.
Factory delegates for customer types are cached in a static array.
This avoids repeated switch expression object-creation branching overhead.
The method remains readable while reducing small but frequent allocations.
### 10.4 Object lifecycle optimization
`DaySimulator` is now reused across loop iterations.
This avoids repeated constructor calls for invariant dependencies.
While not huge, this is a clean correctness-preserving optimization.
## 11. Fix group I: defensive domain constraints
`Store.AddStock` now ignores non-positive quantities.
`Store.AddRevenue` now ignores non-positive values.
`Store.TrySpend` accepts non-positive as no-op success and blocks overspend.
Product matching in `AddStock` is now case-insensitive by name.
This prevents duplicate logical products caused by casing differences.
## 12. Impact on behavior
The simulator now has coherent economy rules.
Inventory cannot be bought without paying.
Revenue is not duplicated.
Invalid console input does not crash core flows.
Menu interactions are more guided and deterministic.
Inventory presentation is clearer for the player.
Performance is improved through lower allocation in hot paths.
## 13. Additional acceptance checklist
1) Build command works from solution root without manual cleanup steps.
2) Build command works from root csproj without duplicate metadata errors.
3) Running from root launches simulator immediately.
4) Running nested project launches the same simulator flow.
5) Invalid menu values do not crash or freeze loop.
6) Invalid numeric order input is always re-prompted safely.
7) Oversized order arithmetic cannot silently overflow.
8) Ordering always deducts cost before stock increase is applied.
9) Daily revenue is posted exactly once to balance.
10) Empty inventory state is clearly communicated to the player.
