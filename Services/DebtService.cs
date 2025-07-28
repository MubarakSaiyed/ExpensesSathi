using ExpensesSathi.Models;
using System.Linq;
using System;
using System.Collections.Generic;


namespace ExpensesSathi.Services
{
    public class DebtService
    {
        public event Action? OnChange;

        private readonly string filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ExpensesSathi",
            "debts.json"
        );
        private List<DebtModel> Debts = new List<DebtModel>();

        public DebtService()
        {
            Debts = Services.JsonFileHelper.LoadFromFile<DebtModel>(filePath);
        }

        private void EnsureDirectoryExists()
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public List<DebtModel> GetAllDebts()
        {
            return Debts;
        }

        // Add Debt - Ensures that a fresh debt is added.
        public void AddDebt(DebtModel debt)
        {
            var newDebt = new DebtModel
            {
                Id = Debts.Count + 1, // Assign unique ID
                Amount = debt.Amount,
                Description = debt.Description,
                DueDate = debt.DueDate,
                Notes = debt.Notes,
                Status = debt.Status,
                ClearedDate = debt.ClearedDate
            };
            Debts.Add(newDebt);
            EnsureDirectoryExists();
            Services.JsonFileHelper.SaveToFile(filePath, Debts);
            OnChange?.Invoke();
        }

        // Update Debt by ID
        public void UpdateDebt(DebtModel debt)
        {
            var existingDebt = Debts.FirstOrDefault(d => d.Id == debt.Id);
            if (existingDebt != null)
            {
                existingDebt.Amount = debt.Amount;
                existingDebt.Description = debt.Description;
                existingDebt.DueDate = debt.DueDate;
                existingDebt.Notes = debt.Notes;
                existingDebt.Status = debt.Status;
                existingDebt.ClearedDate = debt.ClearedDate;
                EnsureDirectoryExists();
                Services.JsonFileHelper.SaveToFile(filePath, Debts);
                OnChange?.Invoke();
            }
        }

        // Mark Debt as Cleared
        public void MarkDebtAsCleared(DebtModel debt)
        {
            var existingDebt = Debts.FirstOrDefault(d => d.Id == debt.Id);
            if (existingDebt != null)
            {
                existingDebt.Status = "Cleared";
                existingDebt.ClearedDate = DateTime.Now;
                EnsureDirectoryExists();
                Services.JsonFileHelper.SaveToFile(filePath, Debts);
                OnChange?.Invoke();
            }
        }

        public decimal GetPendingDebts()
        {
            return Debts.Where(d => d.Status == "Pending").Sum(d => d.Amount);
        }
        
    }
}

