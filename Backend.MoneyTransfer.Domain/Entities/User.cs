using Microsoft.AspNetCore.Identity;

namespace Backend.MoneyTransfer.Domain.Entities;

public class User : IdentityUser<Guid>, IHasDomainEvent
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public DateTimeOffset UpdateDate { get; set; }
    public string LastModifiedBy { get; set; }
    public List<DomainEvent> DomainEvents { get; set; } = new();
    public RecordStatus RecordStatus { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }

    public User()
    {
        if (Balance == 0)
        {
            Balance =  100;
        }
    }
}