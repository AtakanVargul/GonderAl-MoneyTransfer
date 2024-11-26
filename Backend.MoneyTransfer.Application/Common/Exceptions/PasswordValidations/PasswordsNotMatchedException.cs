namespace Backend.MoneyTransfer.Application.Common.Exceptions.PasswordValidations;

public class PasswordsNotMatchedException : ApiException
{
    public PasswordsNotMatchedException()
        : base(ApiErrorCode.PasswordsNotMatched, "Passwords not matched!") { }
}