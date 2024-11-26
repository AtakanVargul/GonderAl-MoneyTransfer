namespace Backend.MoneyTransfer.Domain.Events;

public class UserCreatedEvent : Commons.DomainEvent
{
    public UserCreatedEvent(User user, Dictionary<string, string> parameters = null)
    {
        User = user;
        Parameters = parameters;
    }

    public User User { get; }
    public Dictionary<string, string> Parameters { get; }
}