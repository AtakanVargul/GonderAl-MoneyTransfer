using Backend.MoneyTransfer.Domain.Entities;

namespace Backend.MoneyTransfer.Application.Common.Interfaces;

public interface IJwtHelper
{
    Task<string> GenerateJwtTokenAsync(User user, TimeSpan? expireIn = null);
}