# Run The Code

Unzip GICTest.zip
cd GICTest

dotnet clean BankingSystem/
dotnet restore BankingSystem
dotnet build BankingSystem/
dotnet run --project BankingSystem/

# Run The Test Project
Unzip GICTest.zip
cd GICTest

dotnet clean BankingSystem.Tests/
dotnet test BankingSystem.Tests/

# Run Tests one by one 

# TransactionServiceTests
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.TransactionServiceTests.AddTransaction_ValidDepositTransaction_ReturnsTrue

dotnet test --filter FullyQualifiedName=BankingSystem.Tests.TransactionServiceTests.AddTransaction_ValidWithdrawalTransaction_ReturnsTrue

dotnet test --filter FullyQualifiedName=BankingSystem.Tests.TransactionServiceTests.AddTransaction_InvalidWithdrawalTransaction_ReturnsFalse

dotnet test --filter FullyQualifiedName=BankingSystem.Tests.TransactionServiceTests.AddTransaction_InvalidAmount_ReturnsFalse

dotnet test --filter FullyQualifiedName=BankingSystem.Tests.TransactionServiceTests.GetTransactionsByAccount_ReturnsCorrectTransactions

dotnet test --filter FullyQualifiedName=BankingSystem.Tests.TransactionServiceTests.PrintAllTransactions_ShouldReturnCorrectTransactions

with this command 'dotnet test --filter FullyQulifiedName=<TestCaseName>' all the other tests can be run individually.

# StatementServiceTests

dotnet test --filter FullyQualifiedName=BankingSystem.Tests.StatementServiceTests.GenerateStatement_ShouldReturnCorrectStatement

with this command 'dotnet test --filter FullyQulifiedName=<TestCaseName>' all the other tests can be run individually.

# InterestServiceTests
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.InterestServiceTests.AddInterestRule_ValidRule_ReturnsTrue

dotnet test --filter FullyQualifiedName=BankingSystem.Tests.InterestServiceTests.AddInterestRule_InvalidRate_ReturnsFalse

dotnet test --filter FullyQualifiedName=BankingSystem.Tests.InterestServiceTests.CalculateInterest_ValidTransactions_ReturnsCorrectInterest

dotnet test --filter FullyQualifiedName=BankingSystem.Tests.InterestServiceTests.PrintIntRules_ShouldReturnCorrectRules

with this command 'dotnet test --filter FullyQulifiedName=<TestCaseName>' all the other tests can be run individually.







