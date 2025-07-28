using System;
using System.Collections.Generic;
using System.Linq;
using ExpensesSathi.Models;

using ExpensesSathi.Services;
using System.IO;

public class TransactionService
{
    private readonly string filePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ExpensesSathi",
        "transactions.json"
    );
    public List<Transaction> Transactions { get; private set; } = new List<Transaction>();
    public event Action OnChange;


    public TransactionService()
    {
        // Load from JSON file on startup
        Transactions = JsonFileHelper.LoadFromFile<Transaction>(filePath);
    }

    private void EnsureDirectoryExists()
    {
        var dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }



    public void AddTransaction(Transaction transaction)
    {
        Console.WriteLine($"TransactionService.AddTransaction called with: ID={transaction.Id}, Amount={transaction.Amount}, Type={transaction.Type}");
        EnsureDirectoryExists();
        Transactions.Add(transaction);
        Console.WriteLine($"Transaction added to list. Total count: {Transactions.Count}");
        JsonFileHelper.SaveToFile(filePath, Transactions);
        Console.WriteLine($"Transaction saved to file: {filePath}");
        NotifyStateChanged();
        Console.WriteLine("StateChanged notification sent");
    }

    public void UpdateTransaction(Transaction transaction)
    {
        var existingTransaction = Transactions.FirstOrDefault(t => t.Description == transaction.Description);
        if (existingTransaction != null)
        {
            existingTransaction.Date = transaction.Date;
            existingTransaction.Amount = transaction.Amount;
            existingTransaction.Type = transaction.Type;
            existingTransaction.Tags = transaction.Tags;
            existingTransaction.Notes = transaction.Notes;
            existingTransaction.Status = transaction.Status;

            JsonFileHelper.SaveToFile(filePath, Transactions);
            NotifyStateChanged();
        }
    }

    public void DeleteTransaction(Transaction transaction)
    {
        Transactions.Remove(transaction);
        JsonFileHelper.SaveToFile(filePath, Transactions);
        NotifyStateChanged();
    }

    public List<Transaction> GetAllTransactions()
    {
        return Transactions;
    }

    public List<Transaction> GetTransactionsByDateRange(DateTime? start, DateTime? end)
    {
        return Transactions.Where(t =>
            (!start.HasValue || t.Date >= start.Value) &&
            (!end.HasValue || t.Date <= end.Value)
        ).ToList();
    }

    public decimal GetAvailableBalance(DateTime? start, DateTime? end)
    {
        var filtered = GetTransactionsByDateRange(start, end);
        return filtered.Where(t => t.Type == "credit").Sum(t => t.Amount) - filtered.Where(t => t.Type == "debit").Sum(t => t.Amount);
    }

    public decimal GetTotalInflows(DateTime? start, DateTime? end)
    {
        var filtered = GetTransactionsByDateRange(start, end);
        return filtered.Where(t => t.Type == "credit").Sum(t => t.Amount);
    }

    public decimal GetTotalOutflows(DateTime? start, DateTime? end)
    {
        var filtered = GetTransactionsByDateRange(start, end);
        return filtered.Where(t => t.Type == "debit").Sum(t => t.Amount);
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    public decimal GetAvailableBalance()
    {
        return Transactions.Where(t => t.Type == "credit").Sum(t => t.Amount) - Transactions.Where(t => t.Type == "debit").Sum(t => t.Amount);
    }

    public decimal GetTotalInflows()
    {
        return Transactions.Where(t => t.Type == "credit").Sum(t => t.Amount);
    }

    public decimal GetTotalOutflows()
    {
        return Transactions.Where(t => t.Type == "debit").Sum(t => t.Amount);
    }
}