using Backend.MoneyTransfer.Domain.Commons;
using Backend.MoneyTransfer.Domain.Entities.BaseModels;
using Backend.MoneyTransfer.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.MoneyTransfer.Domain.Entities;

[Table("Transaction", Schema = "Transfer")]
public class Transaction : AuditableEntity, ITrackChange, IHasDomainEvent
{
    public Guid ReceiverId { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset TransactionDate { get; set; }
    public string Description { get; set; }
    public TransactionStatus Status { get; set; }
    public List<DomainEvent> DomainEvents { get; set; } = new();

    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}