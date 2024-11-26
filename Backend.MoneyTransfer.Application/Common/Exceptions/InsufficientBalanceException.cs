namespace Backend.MoneyTransfer.Application.Common.Exceptions;

public class InsufficientBalanceException : GenericException
{
    public InsufficientBalanceException(string message)
        : base(ApiErrorCode.InsufficientBalance, message)
    {
    }

    public InsufficientBalanceException(decimal balance, decimal transactionAmount)
        : base(ApiErrorCode.InsufficientBalance, $"Insufficient balance. Current balance: {balance}, Transaction amount: {transactionAmount}.")
    {
    }
}