namespace Backend.MoneyTransfer.Application.Common.Exceptions.PasswordValidations;

public class InvalidRegisterException : ApiException
{
    public InvalidRegisterException()
        : base(ApiErrorCode.InvalidRegister, "InvalidRegisterException") { }
}