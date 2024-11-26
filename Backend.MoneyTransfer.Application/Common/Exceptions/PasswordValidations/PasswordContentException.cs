﻿namespace Backend.MoneyTransfer.Application.Common.Exceptions.PasswordValidations;

public class PasswordContentException : ApiException
{
    public PasswordContentException()
        : base(ApiErrorCode.PasswordContent, "Password must contains only numbers!") { }
}