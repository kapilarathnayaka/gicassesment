using System;
using System.Collections.Generic;
using BankingSystem.Models;
using BankingSystem.Services;
using Xunit;
using System.Diagnostics;
using BankingSystem.Util;

namespace BankingSystem.Tests
{
    public partial class StatementServiceTests
    {
        private readonly TransactionService _transactionService;
        private readonly InterestService _interestService;
        private readonly StatementService _statementService;

        public StatementServiceTests()
        {
            _transactionService = new TransactionService();
            _interestService = new InterestService();
            _statementService = new StatementService(_transactionService, _interestService);
        }

        [Fact]
        public void GenerateStatement_ShouldReturnCorrectStatement()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Date = "20230501", Account = "AC001", Type = "D", Amount = 100.00m, TransactionId = "20230505-01" },
                new Transaction { Date = "20230601", Account = "AC001", Type = "D", Amount = 150.00m, TransactionId = "20230601-01" },
                new Transaction { Date = "20230626", Account = "AC001", Type = "W", Amount = 20.00m, TransactionId = "20230626-01" },
                new Transaction { Date = "20230626", Account = "AC001", Type = "W", Amount = 100.00m, TransactionId = "20230626-02" }
            };

            foreach (var transaction in transactions)
            {
                _transactionService.AddTransaction(transaction);
            }
            var intersetRules = new List<InterestRule>
            {
                new InterestRule { Date = "20230101", RuleId = "RULE01", Rate = 1.95m },
                new InterestRule { Date = "20230520", RuleId = "RULE02", Rate = 1.90m },
                new InterestRule { Date = "20230615", RuleId = "RULE03", Rate = 2.20m }
            };

            foreach (var interestrule in intersetRules)
            {
                _interestService.AddInterestRule(interestrule);
            }

            // Act
            var statement = _statementService.GenerateStatement("AC001", "202306");

            // Assert
            var expectedStatement = "Account: AC001\n" +
                                    "| Date     | Txn Id      | Type | Amount | Balance |\n" +
                                    "| 20230601 | 20230601-01 | D    | 150.00 |  250.00 |\n" +
                                    "| 20230626 | 20230626-01 | W    |  20.00 |  230.00 |\n" +
                                    "| 20230626 | 20230626-02 | W    | 100.00 |  130.00 |\n" +
                                    "| 20230630 |             | I    |   0.39 |  130.39 |\n";
            
            Assert.Equal(expectedStatement, statement);
        }

        [Fact]
    public void GenerateStatement_WithZeroTransactions_ShouldReturnEmptyStatement()
    {
        // Act
        var statement = _statementService.GenerateStatement("AC002", "202306");

        // Assert
        var expectedStatement = "Account: AC002 No transactions found.";
        Assert.Equal(expectedStatement, statement);
    }

    [Fact]
    public void GenerateStatement_WithLargeValues_ShouldHandleProperly()
    {
        // Arrange
        _transactionService.AddTransaction(new Transaction
        {
            Date = "20230701",
            Account = "AC003",
            Type = "D",
            Amount = 1000000000m,
            TransactionId = "20230701-01"
        });

        // Act
        var statement = _statementService.GenerateStatement("AC003", "202307");

        // Assert
        Assert.Contains("1000000000.00", statement);
    }

    [Fact]
    public void GenerateStatement_WithUpperLimitValues_ShouldNotFail()
    {
        // Arrange
        _transactionService.AddTransaction(new Transaction
        {
            Date = "20230710",
            Account = "AC004",
            Type = "D",
            Amount = decimal.MaxValue,
            TransactionId = "20230710-01"
        });

        // Act
        var statement = _statementService.GenerateStatement("AC004", "202307");
        
        // Assert
        Assert.Contains(decimal.MaxValue.ToString(), statement);
    }

    [Fact]
    public void GenerateStatement_WithLowerLimitValues_ShouldNotFail()
    {
        // Arrange
        var receivingResponse = _transactionService.AddTransaction(new Transaction
        {
            Date = "20230712",
            Account = "AC005",
            Type = "W",
            Amount = decimal.MinValue,
            TransactionId = "20230712-01"
        });

        var expectedReceidingResponse = OperationResult.InvalidInput;
        Assert.Equal(expectedReceidingResponse, receivingResponse);

        // Act
        var statement = _statementService.GenerateStatement("AC005", "202307");

        // Assert
         var expectedStatement = "Account: AC005 No transactions found.";
        Assert.Contains(expectedStatement, statement);

    }

    [Fact]
    public void GenerateStatement_Performance_ShouldBeFast()
    {
        //Arrange
        var account = "AC006";
        for (int i = 0; i < 100; i++)
        {
            _transactionService.AddTransaction(new Transaction
            {
                Date = "20230712",
                Account = account,
                Type = "D",
                Amount = 100m,
                TransactionId = $"20230712-{i:D5}"
            });
        }

        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act
        var statement = _statementService.GenerateStatement(account, "202307");

        stopwatch.Stop();

        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 50, $"Performance test failed: Took {stopwatch.ElapsedMilliseconds}ms");
    }

    }
}
