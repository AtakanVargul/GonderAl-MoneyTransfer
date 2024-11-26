using Backend.MoneyTransfer.Application.Common.Interfaces;

namespace Backend.MoneyTransfer.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    public Guid UserId { get; }
}