namespace Backend.MoneyTransfer.Application.Common.Exceptions;

public class LoginFailedException : ApiException
{
    public LoginFailedException()
        : base(ApiErrorCode.LoginFailed, "Login failed! Check your username and password.") { }
}