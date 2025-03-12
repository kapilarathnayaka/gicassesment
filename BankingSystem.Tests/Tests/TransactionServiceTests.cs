using System;
using System.Collections.Generic;
using BankingSystem.Models;
using BankingSystem.Services;
using BankingSystem.Util;
using Xunit;

namespace BankingSystem.Tests
{
    public partial class TransactionServiceTests
    {
        [Fact]
        public void AddTransaction_ValidDepositTransaction_ReturnsTrue()
        {
            // Arrange
            var transactionService = new TransactionService();
            var transaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = 100.00m
            };

            // Act
            var result = transactionService.AddTransaction(transaction);

            // Assert
            Assert.Equal(OperationResult.Success, result);
        }

        [Fact]
        public void AddTransaction_ValidWithdrawalTransaction_ReturnsTrue()
        {
            // Arrange
            var transactionService = new TransactionService();
            var depositTransaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = 100.00m
            };
            transactionService.AddTransaction(depositTransaction);

            var withdrawalTransaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "W",
                Amount = 50.00m
            };

            // Act
            var result = transactionService.AddTransaction(withdrawalTransaction);

            // Assert
            Assert.Equal(OperationResult.Success, result);
        }

        [Fact]
        public void AddTransaction_InvalidWithdrawalTransaction_ReturnsFalse()
        {
            // Arrange
            var transactionService = new TransactionService();
            var withdrawalTransaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "W",
                Amount = 50.00m
            };

            // Act
            var result = transactionService.AddTransaction(withdrawalTransaction);

            // Assert
            Assert.Equal(OperationResult.InvalidWithdrawFirst, result);
        }

        [Fact]
        public void AddTransaction_InvalidAmount_ReturnsFalse()
        {
            // Arrange
            var transactionService = new TransactionService();
            var transaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = -100.00m
            };

            // Act
            var result = transactionService.AddTransaction(transaction);

            // Assert
            Assert.Equal(OperationResult.InvalidInput, result);
        }

        [Fact]
        public void GetTransactionsByAccount_ReturnsCorrectTransactions()
        {
            // Arrange
            var transactionService = new TransactionService();
            var transaction1 = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = 100.00m
            };
            var transaction2 = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "W",
                Amount = 50.00m
            };
            transactionService.AddTransaction(transaction1);
            transactionService.AddTransaction(transaction2);

            // Act
            var transactions = transactionService.GetTransactionsByAccount("AC001");

            // Assert
            Assert.Equal(2, transactions.Count);
            Assert.Contains(transactions, t => t.TransactionId == "20230626-01" && t.Type == "D" && t.Amount == 100.00m);
            Assert.Contains(transactions, t => t.TransactionId == "20230626-02" && t.Type == "W" && t.Amount == 50.00m);
        }

        [Fact]
        public void PrintAllTransactions_ShouldReturnCorrectTransactions()
        {
            var transactionService = new TransactionService();
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Date = "20230505", Account = "AC001", Type = "D", Amount = 100.00m, TransactionId = "20230505-01" },
                new Transaction { Date = "20230601", Account = "AC001", Type = "D", Amount = 150.00m, TransactionId = "20230601-01" },
                new Transaction { Date = "20230626", Account = "AC001", Type = "W", Amount = 20.00m, TransactionId = "20230626-01" },
                new Transaction { Date = "20230626", Account = "AC001", Type = "W", Amount = 100.00m, TransactionId = "20230626-02" }
            };

            foreach (var transaction in transactions)
            {
                transactionService.AddTransaction(transaction);
            }

            // Act
            var transactionsAll = transactionService.PrintTransactions("AC001", "202306");

            // Assert
            var expectedTranactions = "Account: AC001\n" +
                                    "| Date     | Txn Id      | Type | Amount |\n" +
                                    "| 20230505 | 20230505-01 | D    | 100.00 |\n" +
                                    "| 20230601 | 20230601-01 | D    | 150.00 |\n" +
                                    "| 20230626 | 20230626-01 | W    |  20.00 |\n" +
                                    "| 20230626 | 20230626-02 | W    | 100.00 |\n";

            Assert.Equal(expectedTranactions, transactionsAll);
        }

        [Fact]
        public void AddTransaction_ZeroDepositAmount_ReturnsFalse()
        {
            var transactionService = new TransactionService();
            var transaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = 0m  // Zero deposit amount
            };

            var result = transactionService.AddTransaction(transaction);

            Assert.Equal(OperationResult.InvalidInput, result);
        }

        [Fact]
        public void AddTransaction_ZeroWithdrawalAmount_ReturnsFalse()
        {
            var transactionService = new TransactionService();
            var transaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "W",
                Amount = 0m  // Zero withdrawal amount
            };

            var result = transactionService.AddTransaction(transaction);

            Assert.Equal(OperationResult.InvalidInput, result);
        }

        [Fact]
        public void AddTransaction_MinDepositAmount_Succeeds()
        {
            var transactionService = new TransactionService();
            var transaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = 0.01m  // Smallest valid deposit
            };

            var result = transactionService.AddTransaction(transaction);

            Assert.Equal(OperationResult.Success, result);
        }

        [Fact]
        public void AddTransaction_MinWithdrawalAmount_Succeeds()
        {
            var transactionService = new TransactionService();
            var deposit = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = 100m
            };
            transactionService.AddTransaction(deposit);

            var withdrawal = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "W",
                Amount = 0.01m  // Smallest valid withdrawal
            };

            var result = transactionService.AddTransaction(withdrawal);

            Assert.Equal(OperationResult.Success, result);
        }

        [Fact]
        public void AddTransaction_MaxDepositAmount_Succeeds()
        {
            var transactionService = new TransactionService();
            var transaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = decimal.MaxValue // Maximum possible deposit
            };

            var result = transactionService.AddTransaction(transaction);

            Assert.Equal(OperationResult.Success, result);
        }

        [Fact]
        public void AddTransaction_MaxWithdrawalAmount_Succeeds()
        {
            var transactionService = new TransactionService();
            var deposit = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = decimal.MaxValue
            };
            transactionService.AddTransaction(deposit);

            var withdrawal = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "W",
                Amount = decimal.MaxValue // Maximum possible withdrawal
            };

            var result = transactionService.AddTransaction(withdrawal);

            Assert.Equal(OperationResult.Success, result);
        }

        [Fact]
        public void AddTransaction_NearUpperLimitDeposit_Succeeds()
        {
            var transactionService = new TransactionService();
            var transaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = decimal.MaxValue - 1 // Just below upper limit
            };

            var result = transactionService.AddTransaction(transaction);

            Assert.Equal(OperationResult.Success, result);
        }

        [Fact]
        public void AddTransaction_NearUpperLimitWithdrawal_Succeeds()
        {
            var transactionService = new TransactionService();
            var deposit = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = decimal.MaxValue
            };
            transactionService.AddTransaction(deposit);

            var withdrawal = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "W",
                Amount = decimal.MaxValue - 1 // Just below upper limit
            };

            var result = transactionService.AddTransaction(withdrawal);

            Assert.Equal(OperationResult.Success, result);
        }

        [Fact]
        public void AddTransaction_ThousandTransactions_PerformsEfficiently()
        {
            var transactionService = new TransactionService();
            var account = "AC001";
            var deposit = new Transaction
            {
                Date = "20230626",
                Account = account,
                Type = "D",
                Amount = 1000000m
            };
            transactionService.AddTransaction(deposit);

            var transactions = new List<Transaction>();
            for (int i = 0; i < 1000; i++)
            {
                transactions.Add(new Transaction
                {
                    Date = "20230626",
                    Account = account,
                    Type = "W",
                    Amount = 1m
                });
            }

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            foreach (var transaction in transactions)
            {
                transactionService.AddTransaction(transaction);
            }

            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds < 1000, "Transaction processing took too long");
        }

        [Fact]
        public void GetTransactionsByAccount_HugeData_PerformsEfficiently()
        {
            var transactionService = new TransactionService();
            var account = "AC001";

            for (int i = 0; i < 10000; i++)
            {
                var transaction = new Transaction
                {
                    Date = "20230626",
                    Account = account,
                    Type = i % 2 == 0 ? "D" : "W",
                    Amount = 10m
                };
                transactionService.AddTransaction(transaction);
            }

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var transactions = transactionService.GetTransactionsByAccount(account);
            stopwatch.Stop();

            Assert.True(transactions.Count == 10000, "Transaction count mismatch");
            Assert.True(stopwatch.ElapsedMilliseconds < 2000, "Fetching transactions took too long");
        }

        [Fact]
        public void AddTransaction_FirstTransactionCannotBeWithdrawal()
        {
            var transactionService = new TransactionService();
            var withdrawalTransaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "W",
                Amount = 50.00m
            };

            var result = transactionService.AddTransaction(withdrawalTransaction);
            Assert.Equal(OperationResult.InvalidWithdrawFirst, result);
        }

        [Fact]
        public void AddTransaction_WithdrawalCannotMakeBalanceNegative()
        {
            var transactionService = new TransactionService();
            var depositTransaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = 100.00m
            };
            transactionService.AddTransaction(depositTransaction);

            var withdrawalTransaction = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "W",
                Amount = 150.00m // Should fail because balance is 100
            };

            var result = transactionService.AddTransaction(withdrawalTransaction);
            Assert.Equal(OperationResult.InsufficientFunds, result);
        }

        [Fact]
        public void AddTransaction_GeneratesCorrectTransactionIdFormat()
        {
            var transactionService = new TransactionService();
            var transaction1 = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "D",
                Amount = 100.00m
            };
            var transaction2 = new Transaction
            {
                Date = "20230626",
                Account = "AC001",
                Type = "W",
                Amount = 50.00m
            };

            transactionService.AddTransaction(transaction1);
            transactionService.AddTransaction(transaction2);

            var transactions = transactionService.GetTransactionsByAccount("AC001");

            Assert.Contains(transactions, t => t.TransactionId == "20230626-01");
            Assert.Contains(transactions, t => t.TransactionId == "20230626-02");
        }
    }
}