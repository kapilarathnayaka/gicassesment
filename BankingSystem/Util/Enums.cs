namespace BankingSystem.Util
{
    public enum OperationResult
    {
        Success,
        Error,
        NotFound,
        InvalidInput,
        InvalidWithdrawFirst,
        InsufficientFunds,
        InvalidInterestRate
    }
}