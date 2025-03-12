using System;
using System.Collections.Generic;
using System.Linq;
using BankingSystem.Models;
using BankingSystem.Util;

namespace BankingSystem.Services
{
    public class TransactionService
    {
        private readonly List<Transaction> _transactions = new List<Transaction>();
        private readonly Dictionary<string, decimal> _accountBalances = new Dictionary<string, decimal>();

        public OperationResult AddTransaction(Transaction transaction)
        {
            OperationResult result = ValidateTransaction(transaction);
            if (result != OperationResult.Success)
            {
                return result;
            }

            transaction.TransactionId = GenerateTransactionId(transaction.Date);

            _transactions.Add(transaction);

            if (!_accountBalances.ContainsKey(transaction.Account))
            {
                _accountBalances[transaction.Account] = 0;
            }

            if (transaction.Type.ToUpper() == "D")
            {
                _accountBalances[transaction.Account] += transaction.Amount;
            }
            else if (transaction.Type.ToUpper() == "W")
            {
                _accountBalances[transaction.Account] -= transaction.Amount;
            }
            Console.WriteLine($"Transaction added : {transaction.TransactionId} Account : {transaction.Account}");
            return OperationResult.Success;
        }

        private OperationResult ValidateTransaction(Transaction transaction)
        {
            // Validate the transaction amount
            if (transaction.Amount <= 0)
            {
                return OperationResult.InvalidInput;
            }

            // Validate first transaction.It should be a deposit
            if (transaction.Type.ToUpper() == "W" && (!_accountBalances.ContainsKey(transaction.Account)))
            {
                return OperationResult.InvalidWithdrawFirst;
            }

            // Validate if the account has sufficient balance for withdrawal
            if (transaction.Type.ToUpper() == "W" && _accountBalances[transaction.Account] < transaction.Amount)
            {
                return OperationResult.InsufficientFunds;
            }

            return OperationResult.Success; 
        }

        // Generate a unique transaction id based on the date
        private string GenerateTransactionId(string date)
        {
            var count = _transactions.Count(t => t.Date == date) + 1;
            return $"{date}-{count:D2}";
        }


        // Get all transactions for an account
        public List<Transaction> GetTransactionsByAccount(string account)
        {
            return _transactions.Where(t => t.Account == account).ToList();
        }

        // Get the brought forward balance for an account
        public decimal GetBroughtForwardBalance(string account, List<Transaction> transactions, string yearMonth)
        {
            var totalBFBalance = transactions
                .Where(t => t.Account == account &&
                DateTime.ParseExact(t.Date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) <
                DateTime.ParseExact($"{yearMonth}01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture))
                .Sum(t => t.Type.ToUpper() == "D" ? t.Amount : -t.Amount);
            return totalBFBalance;
        }

        // Print all transactions for an account
        public string PrintTransactions(string account, string yearMonth)
        {
            var transactions = GetTransactionsByAccount(account);
            var statement = $"Account: {account}\n";
            statement += "| Date     | Txn Id      | Type | Amount |\n";

           foreach (var transaction in transactions)
            {
                statement += $"| {transaction.Date} | {transaction.TransactionId} | {transaction.Type}    |{transaction.Amount,7:F2} |\n";
            }
            return statement;
        }
    }
}
