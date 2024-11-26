namespace Backend.MoneyTransfer.Application.Common.Models.IdentityConfiguration;

public class TokenSettings
{
    public TokenAuthenticationSettings TokenAuthenticationSettings { get; set; }
    public int TokenExpiryDefaultMinute { get; set; }
}