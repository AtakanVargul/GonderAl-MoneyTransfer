using Backend.MoneyTransfer.Application.Common.Mappings;
using Backend.MoneyTransfer.Domain.Entities;
using Backend.MoneyTransfer.Domain.Enums;

namespace Backend.MoneyTransfer.Application.Features.Transfers.Commands;

public class TransactionResponse : IMapFrom<Transaction>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ReceiverId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public TransactionStatus Status { get; set; }
}