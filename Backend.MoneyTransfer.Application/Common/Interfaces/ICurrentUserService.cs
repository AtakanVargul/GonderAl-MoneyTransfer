﻿namespace Backend.MoneyTransfer.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
}