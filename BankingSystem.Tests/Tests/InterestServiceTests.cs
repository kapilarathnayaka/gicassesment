using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Models;
using BankingSystem.Services;
using Xunit;
using System.Diagnostics;

namespace BankingSystem.Tests
{
    public partial class InterestServiceTests
    {
        private readonly InterestService _interestService;

        public InterestServiceTests()
        {
            _interestService = new InterestService();
        }

        [Fact]
        public void AddInterestRule_ValidRule_ReturnsTrue()
        {
            var rule = new InterestRule
            {
                Date = "20230615",
                RuleId = "RULE03",
                Rate = 2.20m
            };

            var result = _interestService.AddInterestRule(rule);

            Assert.True(result);
        }

        [Fact]
        public void AddInterestRule_InvalidRate_ReturnsFalse()
        {
            var rule = new InterestRule
            {
                Date = "20230615",
                RuleId = "RULE03",
                Rate = 0m
            };

            var result = _interestService.AddInterestRule(rule);

            Assert.False(result);
        }

        [Fact]
        public void CalculateInterest_ValidTransactions_ReturnsCorrectInterest()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Date = "20230501", Account = "AC001", Type = "D", Amount = 100.00m },
                new Transaction { Date = "20230601", Account = "AC001", Type = "D", Amount = 150.00m },
                new Transaction { Date = "20230626", Account = "AC001", Type = "W", Amount = 20.00m },
                new Transaction { Date = "20230626", Account = "AC001", Type = "W", Amount = 100.00m }
            };

            var rule1 = new InterestRule
            {
                Date = "20230101",
                RuleId = "RULE01",
                Rate = 1.95m
            };

            var rule2 = new InterestRule
            {
                Date = "20230520",
                RuleId = "RULE02",
                Rate = 1.90m
            };

            var rule3 = new InterestRule
            {
                Date = "20230615",
                RuleId = "RULE03",
                Rate = 2.20m
            };

            _interestService.AddInterestRule(rule1);
            _interestService.AddInterestRule(rule2);
            _interestService.AddInterestRule(rule3);

            var interest = _interestService.CalculateInterest("AC001", transactions, "202306");

            Assert.Equal(0.39m, interest);
        }

        [Fact]
        public void PrintIntRules_ShouldReturnCorrectRules()
        {
            // Arrange
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
            var actualRules = _interestService.PrintInterestRules();

            var expectedRules = "Interest rules:\n" +
            "| Date     | RuleId | Rate (%) |\n" +
            "| 20230101 | RULE01 |     1.95 |\n" +
            "| 20230520 | RULE02 |     1.90 |\n" +
            "| 20230615 | RULE03 |     2.20 |\n";

            Assert.Equal(expectedRules, actualRules);
        }

        [Fact]
        public void AddInterestRule_NegativeRate_ReturnsFalse()
        {
            var rule = new InterestRule
            {
                Date = "20230615",
                RuleId = "RULE03",
                Rate = -1m
            };

            var result = _interestService.AddInterestRule(rule);

            Assert.False(result);
        }

        [Fact]
        public void AddInterestRule_LargeRate_ReturnsFalse()
        {
            var rule = new InterestRule
            {
                Date = "20230615",
                RuleId = "RULE03",
                Rate = 100.01m
            };

            var result = _interestService.AddInterestRule(rule);

            Assert.False(result);
        }

        [Fact]
        public void AddInterestRule_EmptyRuleId_ReturnsFalse()
        {
            var rule = new InterestRule
            {
                Date = "20230615",
                RuleId = "",
                Rate = 2.20m
            };

            var result = _interestService.AddInterestRule(rule);

            Assert.False(result);
        }

        [Fact]
        public void AddInterestRule_EmptyRuleDate_ReturnsFalse()
        {
            var rule = new InterestRule
            {
                Date = "",
                RuleId = "RULE01",
                Rate = 2.20m
            };

            var result = _interestService.AddInterestRule(rule);

            Assert.False(result);
        }

        [Fact]
        public void CalculateInterest_NoTransactions_ReturnsZero()
        {
            var transactions = new List<Transaction>();
            var interest = _interestService.CalculateInterest("AC001", transactions, "202306");

            Assert.Equal(0m, interest);
        }

        [Fact]
        public void CalculateInterest_ZeroBalance_ReturnsZero()
        {
            var transactions = new List<Transaction>
            {
            new Transaction { Date = "20230601", Account = "AC002", Type = "D", Amount = 0.00m }
            };

            var interest = _interestService.CalculateInterest("AC002", transactions, "202306");
            Assert.Equal(0m, interest);
        }

        [Fact]
        public void PrintInterestRules_ShouldReturnCorrectRules()
        {
            var interestRules = new List<InterestRule>
        {
        new InterestRule { Date = "20230101", RuleId = "RULE01", Rate = 1.95m },
        new InterestRule { Date = "20230520", RuleId = "RULE02", Rate = 1.90m },
        new InterestRule { Date = "20230615", RuleId = "RULE03", Rate = 2.20m }
        };

            foreach (var interestRule in interestRules)
            {
                _interestService.AddInterestRule(interestRule);
            }

            var actualRules = _interestService.PrintInterestRules();

            var expectedRules = "Interest rules:\n" +
            "| Date     | RuleId | Rate (%) |\n" +
            "| 20230101 | RULE01 |     1.95 |\n" +
            "| 20230520 | RULE02 |     1.90 |\n" +
            "| 20230615 | RULE03 |     2.20 |\n";

            Assert.Equal(expectedRules, actualRules);
        }

        [Fact]
        public void PrintInterestRules_EmptyRules_ShouldReturnEmptyMessage()
        {
            var actualRules = _interestService.PrintInterestRules();
            Assert.Equal("No interest rules defined.", actualRules);
        }

        [Fact]
        public void AddInterestRuleSpeed_ShouldBeUnderExpectedTime()
        {
            // Arrange
            TimeSpan expectedTime = TimeSpan.FromMilliseconds(5); // should be under 5 ms

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Act
            _interestService.AddInterestRule(new InterestRule { Date = "20230101", RuleId = "RULE01", Rate = 1.95m });

            stopwatch.Stop();

            // Assert
            TimeSpan actualTime = stopwatch.Elapsed;
            Assert.True(actualTime < expectedTime, $"Transaction took too long: {actualTime.TotalMilliseconds} ms");
        }

        [Fact]
        public void ValidateInterestRate_ValidRate_ReturnsSuccess()
        {
            var rule = new InterestRule
            {
                Date = "20230615",
                RuleId = "RULE01",
                Rate = 5.5m
            };
            var result = _interestService.ValidateInterestRule(rule);
            Assert.Equal(true, result);
        }

        [Fact]
        public void ValidateInterestRate_ZeroRate_ReturnsInvalid()
        {
            var rule = new InterestRule
            {
                Date = "20230615",
                RuleId = "RULE02",
                Rate = 0m
            };
            var result = _interestService.ValidateInterestRule(rule);
            Assert.Equal(false, result);
        }

        [Fact]
        public void ValidateInterestRate_NegativeRate_ReturnsInvalid()
        {
            var rule = new InterestRule
            {
                Date = "20230615",
                RuleId = "RULE03",
                Rate = -10m
            };
            var result = _interestService.ValidateInterestRule(rule);
            Assert.Equal(false, result);
        }

        [Fact]
        public void ValidateInterestRate_UpperLimit100_ReturnsInvalid()
        {
            var rule = new InterestRule
            {
                Date = "20230615",
                RuleId = "RULE04",
                Rate = 100m
            };
            var result = _interestService.ValidateInterestRule(rule);
            Assert.Equal(false, result);
        }

        [Fact]
        public void ValidateInterestRate_JustBelow100_ReturnsSuccess()
        {
            var rule = new InterestRule
            {
                Date = "20230615",
                RuleId = "RULE05",
                Rate = 99.99m
            };
            var result = _interestService.ValidateInterestRule(rule);
            Assert.Equal(true, result);
        }
    }
}
