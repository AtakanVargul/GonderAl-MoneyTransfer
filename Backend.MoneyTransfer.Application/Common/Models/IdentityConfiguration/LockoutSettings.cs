namespace Backend.MoneyTransfer.Application.Common.Models.IdentityConfiguration;

public class LockoutSettings
{
    public bool AllowedForNewUsers { get; set; }
    public int DefaultLockoutTimeSpanInMinutes { get; set; }
    public int MaxFailedAccessAttempts { get; set; }
}