using System;
using System.Collections.Generic;
using System.Linq;
using BankingSystem.Models;

namespace BankingSystem.Services
{
    public class InterestService
    {
        private readonly List<InterestRule> _interestRules = new List<InterestRule>();
        public bool AddInterestRule(InterestRule interestRule)
        {
            if (!ValidateInterestRule(interestRule))
            {
                return false;
            }

            var existingRule = _interestRules.FirstOrDefault(r => r.Date == interestRule.Date);
            if (existingRule != null)
            {
                _interestRules.Remove(existingRule);
            }

            _interestRules.Add(interestRule);
            return true;
        }

        public bool ValidateInterestRule(InterestRule interestRule)
        {
            if (string.IsNullOrEmpty(interestRule.Date) || interestRule.Date.Length != 8)
            {
                Console.WriteLine("Invalid Date.");
                return false;
            }
            if(string.IsNullOrEmpty(interestRule.RuleId))
            {
                return false;
            }
            if (interestRule.Rate <= 0 || interestRule.Rate >= 100)
            {
                return false;
            }

            return true;
        }

        public decimal CalculateInterest(string account, List<Transaction> transactions, string yearMonth)
        {
            var eodBalances = GetEndOfDayBalances(account, transactions, yearMonth);
            var interest = 0m;

            foreach (var eodBalance in eodBalances)
            {
                var rule = GetInterestRule(eodBalance.Date);
                if (rule != null)
                {
                    var days = (eodBalance.EndDate - eodBalance.StartDate).Days + 1;
                    var annualizedInterest = eodBalance.Balance * rule.Rate / 100 * days;
                    interest += annualizedInterest;
                }
            }
            return Math.Round(interest / 365, 2);
        }

        private List<EndOfDayBalance> GetEndOfDayBalances(string account, List<Transaction> transactions, string yearMonth)
        {
            var eodBalances = new List<EndOfDayBalance>();
            try
            {
                var startDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", null);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                //check for invalid account
                if (transactions.Where(t => t.Account == account).Count() == 0)
                {
                    Console.WriteLine("Invalid Account.");
                    return eodBalances;
                }

                var currentBalance = transactions
                    .Where(t => t.Account == account && DateTime.ParseExact(t.Date, "yyyyMMdd", null) < startDate)
                    .Sum(t => t.Type.ToUpper() == "D" ? t.Amount : -t.Amount);

                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    var dailyTransactions = transactions
                        .Where(t => t.Account == account && t.Date == date.ToString("yyyyMMdd"))
                        .ToList();

                    foreach (var transaction in dailyTransactions)
                    {
                        currentBalance += transaction.Type.ToUpper() == "D" ? transaction.Amount : -transaction.Amount;
                    }

                    eodBalances.Add(new EndOfDayBalance
                    {
                        Date = date.ToString("yyyyMMdd"),
                        Balance = currentBalance,
                        StartDate = date,
                        EndDate = date
                    });
                }
                return eodBalances;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Invalid <Year><Month> Format.");
                return eodBalances;
            }
        }

        private InterestRule GetInterestRule(string date)
        {
            return _interestRules
                .Where(r => DateTime.ParseExact(r.Date, "yyyyMMdd", null) <= DateTime.ParseExact(date, "yyyyMMdd", null))
                .OrderByDescending(r => r.Date)
                .FirstOrDefault();
        }

        // Print the interest rules 
        public string PrintInterestRules()
        {
            if(_interestRules.Count == 0)
            {
                return "No interest rules defined.";
            }
            var rates = $"Interest rules:\n";
            rates += "| Date     | RuleId | Rate (%) |\n";
            foreach (var rule in _interestRules)
            {
                rates += $"| {rule.Date} | {rule.RuleId} |  {rule.Rate,7:F2} |\n";
            }
            return rates;
        }

        private class EndOfDayBalance
        {
            public string Date { get; set; }
            public decimal Balance { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
    }
}
