using System;
using System.Collections.Generic;
using System.Linq;
using BankingSystem.Models;

namespace BankingSystem.Services
{
    public class StatementService
    {
        private readonly TransactionService _transactionService;
        private readonly InterestService _interestService;

        public StatementService(TransactionService transactionService, InterestService interestService)
        {
            _transactionService = transactionService;
            _interestService = interestService;
        }

        public string GenerateStatement(string account, string yearMonth)
        {
            var transactions = _transactionService.GetTransactionsByAccount(account);

            Console.WriteLine($"Transactions : {transactions.Count} for account {account}");

            if(transactions == null || transactions.Count == 0)
            {
                return $"Account: {account} No transactions found.";
            }
            var statement = $"Account: {account}\n";
            statement += "| Date     | Txn Id      | Type | Amount | Balance |\n";

            var balance = 0m;
            var bfBalance = _transactionService.GetBroughtForwardBalance(account, transactions, yearMonth);
            balance += bfBalance;
          
            foreach (var transaction in transactions.Where(t => t.Date.StartsWith(yearMonth)))
            {
                balance += transaction.Type.ToUpper() == "D" ? transaction.Amount : -transaction.Amount;
                statement += $"| {transaction.Date} | {transaction.TransactionId} | {transaction.Type}    |{transaction.Amount,7:F2} | {balance,7:F2} |\n";
            }

            var interest = _interestService.CalculateInterest(account, transactions, yearMonth);
            if (interest > 0)
            {
                balance += interest;
                statement += $"| {yearMonth}30 |             | I    |{interest,7:F2} | {balance,7:F2} |\n";
            }

            return statement;
        }
    }
}
