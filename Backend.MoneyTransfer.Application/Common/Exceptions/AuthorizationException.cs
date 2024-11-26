
namespace Backend.MoneyTransfer.Application.Common.Exceptions;

public class AuthorizationException : UnauthorizedAccessException
{
    public readonly string Code;

    public AuthorizationException(string code, string message)
        : base(message)
    {
        Code = code;
    }
}