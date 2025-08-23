# lab-dotnet

A collection of small experiments and playground projects using **.NET 8**.  
Each folder explores a single topic (EF Core, Minimal APIs, Rx.NET, Dapper, etc).  
These are not production apps — just experiments to learn, compare, and share.

## 🔍 Structure
- Each experiment lives in its own folder: `YYYY-MM-topic-slug`
- Contains a `README.md` (what/why/how), `src/`, and sometimes `test/`
- CI builds and runs tests for all experiments automatically

## 📂 List
- `2025-08-ef-bulk-insert` — Example

*(More experiments coming soon…)*

## 🚀 How to Run
```bash
# build all projects
dotnet build

# run a specific experiment
cd 2025-08-ef-bulk-insert/src
dotnet run
