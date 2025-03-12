# Run The Code

Clone repository 

```bash
git clone https://github.com/kapilarathnayaka/gicassesment.git
```
cd BankingSystem

```bash
dotnet clean
dotnet restore
dotnet build
dotnet run
```

cd ..
cd BankingSystem.Tests

```bash
dotnet clean
dotnet test
```

# Run Individual Tests 

## TransactionServiceTests

```bash
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.TransactionServiceTests.AddTransaction_ValidDepositTransaction_ReturnsTrue
```
```bash
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.TransactionServiceTests.AddTransaction_ValidWithdrawalTransaction_ReturnsTrue
```
```bash
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.TransactionServiceTests.AddTransaction_InvalidWithdrawalTransaction_ReturnsFalse
```
```bash
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.TransactionServiceTests.AddTransaction_InvalidAmount_ReturnsFalse
```
```bash
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.TransactionServiceTests.GetTransactionsByAccount_ReturnsCorrectTransactions
```
```bash
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.TransactionServiceTests.PrintAllTransactions_ShouldReturnCorrectTransactions
```
with this command 'dotnet test --filter FullyQulifiedName=[TestCaseName] (without []) all the other tests can be run individually.

## StatementServiceTests

```bash
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.StatementServiceTests.GenerateStatement_ShouldReturnCorrectStatement
```
with this command 'dotnet test --filter FullyQulifiedName=[TestCaseName] (without []) all the other tests can be run individually.

## InterestServiceTests

```bash
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.InterestServiceTests.AddInterestRule_ValidRule_ReturnsTrue
```
```bash
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.InterestServiceTests.AddInterestRule_InvalidRate_ReturnsFalse
```
```bash
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.InterestServiceTests.CalculateInterest_ValidTransactions_ReturnsCorrectInterest
```
```bash
dotnet test --filter FullyQualifiedName=BankingSystem.Tests.InterestServiceTests.PrintIntRules_ShouldReturnCorrectRules
```
with this command 'dotnet test --filter FullyQulifiedName=[TestCaseName]' (without []) all the other tests can be run individually.

-EOD-




