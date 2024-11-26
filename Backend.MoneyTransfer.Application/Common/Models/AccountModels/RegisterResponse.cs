namespace Backend.MoneyTransfer.Application.Common.Models.AccountModels;

public class RegisterResponse
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
}