namespace Backend.MoneyTransfer.Application.Common.Exceptions;

public class GenericException : Exception
{
    public readonly string Code;

    public GenericException(string code, string message)
        : base(message)
    {
        Code = code;
    }
}