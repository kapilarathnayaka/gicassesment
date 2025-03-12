using System;
using System.Collections.Generic;
using BankingSystem.Models;
using BankingSystem.Services;
using BankingSystem.Util;

namespace BankingSystem
{
    class Program
    {
        static void Main(string[] args)
        {


            var transactionService = new TransactionService();
            var interestService = new InterestService();
            var statementService = new StatementService(transactionService, interestService);
           
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
                Console.WriteLine("[T] Input transactions");
                Console.WriteLine("[I] Define interest rules");
                Console.WriteLine("[P] Print statement");
                Console.WriteLine("[Q] Quit");
                Console.Write("> ");

                var input = Console.ReadLine().ToUpper();

                switch (input)
                {
                    case "T":
                        InputTransactions(transactionService);
                        break;
                    case "I":
                        DefineInterestRules(interestService);
                        break;
                    case "P":
                        PrintStatement(statementService);
                        break;
                    case "Q":
                        Console.WriteLine("Thank you for banking with AwesomeGIC Bank.");
                        Console.WriteLine("Have a nice day!");
                        System.Threading.Thread.Sleep(2000);
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void InputTransactions(TransactionService transactionService)
        {
            while (true)
            {
                Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format");
                Console.WriteLine("(or enter blank to go back to main menu):");
                Console.Write("> ");

                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }

                var validEntry = Util.Util.ValidateTransaction(input);

                // Invalid entry. go to main menu again.
                if (!validEntry)
                {
                    continue;
                }

                var parts = input.Split(' ');

                var date = parts[0];
                var account = parts[1];
                var type = parts[2];
                var amount = decimal.Parse(parts[3]);

                var transaction = new Transaction
                {
                    Date = date,
                    Account = account,
                    Type = type,
                    Amount = amount
                };

                var result = transactionService.AddTransaction(transaction);
                if (result == OperationResult.Success)
                {
                    Console.WriteLine("Transaction added successfully.");
                    var allTransactions = transactionService.PrintTransactions(transaction.Account, transaction.Date.Substring(0, 6));
                    Console.WriteLine(allTransactions);
                }
                else
                {
                    if(result == OperationResult.InvalidWithdrawFirst)
                    {
                        Console.WriteLine("First transction cannot be a withdrawal.");
                    }
                    else if(result == OperationResult.InsufficientFunds)
                    {
                        Console.WriteLine("Insufficient Funds.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add transaction. Please recheck.");
                    }
                }
            }
        }

        static void DefineInterestRules(InterestService interestService)
        {
            while (true)
            {
                Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format");
                Console.WriteLine("(or enter blank to go back to main menu):");
                Console.Write("> ");

                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }
                
                var validEntry = Util.Util.ValidateInterestRule(input);

                // Invalid entry. go to main menu again.
                if (!validEntry)
                {
                    continue;
                }

                var parts = input.Split(' ');
                var date = parts[0];
                var ruleId = parts[1];
                var rate = decimal.Parse(parts[2]);

                var interestRule = new InterestRule
                {
                    Date = date,
                    RuleId = ruleId,
                    Rate = rate
                };

                var result = interestService.AddInterestRule(interestRule);
                if (result)
                {
                    Console.WriteLine("Interest rule added successfully.");
                    var rates = interestService.PrintInterestRules();
                    Console.WriteLine(rates);
                }
                else
                {
                    Console.WriteLine("Failed to add interest rule. Please check the constraints.");
                }
            }
        }

        static void PrintStatement(StatementService statementService)
        {
            while (true)
            {
                Console.WriteLine("Please enter account and month to generate the statement <Account> <Year><Month>");
                Console.WriteLine("(or enter blank to go back to main menu):");
                Console.Write("> ");

                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }

                var validEntry = Util.Util.ValidateStatementEntry(input);

                // Invalid entry. go to main menu again.
                if (!validEntry)
                {
                    continue;
                }

                var parts = input.Split(' ');
                if (parts.Length != 2)
                {
                    Console.WriteLine("Invalid input format. Please try again.");
                    continue;
                }

                var account = parts[0];
                var yearMonth = parts[1];

                var statement = statementService.GenerateStatement(account, yearMonth);
                Console.WriteLine(statement);
            }
        }
    }
}
