namespace Backend.MoneyTransfer.Application.Common.Exceptions;

public class AlreadyInUseException : ApiException
{
    public AlreadyInUseException(string userName)
        : base(ApiErrorCode.AlreadyInUse, $"\"{userName}\" already taken!") { }
}