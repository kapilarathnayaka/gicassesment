using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BankingSystem.Util
{
    public static class Util
    {
        public static bool ValidateTransaction(string input)
        {
            string pattern = @"^(\d{8})\s+([A-Za-z0-9]+)\s+([WD])\s+(\d{1,8}\.?\d+?)$";

            Match match = Regex.Match(input, pattern);

            // Validate if the input matches the pattern
            if (match.Success)
            {
                // Validate the date
                string datePart = match.Groups[1].Value;

                // Validate if the first 8 digits represent a valid date
                if (!DateTime.TryParseExact(datePart, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out _))
                {
                    Console.WriteLine("Invalid date.Date should be in yyyyMMdd format.");
                    return false;
                }

                /*
                Provisions for Account Number Validation. 
                Free format No validation needed in this development stage
                string accountNumberPart = match.Groups[2].Value;
                */

                // Validate if the transaction type is either W or D
                string transactionTypePart = match.Groups[3].Value;

                if (transactionTypePart.ToUpper() != "W" && transactionTypePart.ToUpper() != "D")
                {
                    Console.WriteLine("Invalid transaction type.Transaction type should be either W (Withdrawal) or D (Deposit).");
                    return false;
                }

                // Validate if the amount is a valid amount with 2 decimal places
                string amountPart = match.Groups[4].Value;

                // If no decimal is added add .00 to the end
                if (!amountPart.Contains('.'))
                {
                    amountPart += ".00";
                }

                // Trim amount part to 2 decimal places
                string decimalPart = amountPart.Substring(amountPart.IndexOf('.') + 1);

                if (decimalPart.Length > 2)
                {
                    amountPart = amountPart.Substring(0, amountPart.Length - (decimalPart.Length - 2));
                }

                // Validate amount is less than or equal to 0
                if (decimal.Parse(amountPart) <= 0)
                {
                    Console.WriteLine("Invalid amount.Amount should be greater than 0.");
                    return false;
                }

                // Validate if the amount is a valid decimal
                if (!decimal.TryParse(amountPart, out _))
                {
                    Console.WriteLine("Invalid amount.");
                    return false;
                }
            }
            else
            {
                //If the main pattern is not matched
                Console.WriteLine("Invalid input format. Please try again.");
                return false;
            }
            return true;
        }

        public static bool ValidateInterestRule(string input)
        {
            string pattern = @"^(\d{8})\s+([A-Za-z0-9]+)\s+((?:[0-9]|[1-9][0-9])(?:\.\d{1,2})?)$";

            Match match = Regex.Match(input, pattern);

            // Validate if the input matches the pattern
            if (match.Success)
            {
                // Validate the date
                string datePart = match.Groups[1].Value;

                // Validate if the first 8 digits represent a valid date
                if (!DateTime.TryParseExact(datePart, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out _))
                {
                    Console.WriteLine("Invalid date.Date should be in yyyyMMdd format.");
                    return false;
                }

                /*
                Provisions for Rule Name Validation. 
                Free format No validation needed in this development stage
                string ruleNamePart = match.Groups[2].Value;
                */

                // Validate interest rate part
                string interestRate = match.Groups[3].Value;

                // Validate amount is less than or equal to 0
                if (decimal.Parse(interestRate) <= 0 || decimal.Parse(interestRate) >= 100)
                {
                    Console.WriteLine("Invalid interest rate. Interest rate should be greater than 0 and less than 100.");
                    return false;
                }
            }
            else
            {
                //If the main pattern is not matched
                Console.WriteLine("Invalid input format. Please try again.");
                return false;
            }
            return true;
        }

        public static bool ValidateStatementEntry(string input)
        {
            string pattern = @"^([A-Za-z0-9]+)\s+(\d{6})$";

            Match match = Regex.Match(input, pattern);

            // Validate if the input matches the pattern
            if (match.Success)
            {
                // Validate the account number
                string accountNumberPart = match.Groups[1].Value;

                // Validate if the account number is alphanumeric
                if (!accountNumberPart.All(char.IsLetterOrDigit))
                {
                    Console.WriteLine("Invalid account number.Account number should be alphanumeric.");
                    return false;
                }

                //Valudare the year month
                string yearMonthPart = match.Groups[2].Value;
                //add01 to the end of the year month

                //parse and check the datemonth
                if (!DateTime.TryParseExact($"{yearMonthPart}01", "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out _))
                {
                    Console.WriteLine("Invalid year month.Year month should be in yyyyMM format.");
                    return false;
                }
            }
            else
            {
                //If the main pattern is not matched
                Console.WriteLine("Invalid input format. Please try again.");
                return false;
            }
            return true;
        }
    }
}