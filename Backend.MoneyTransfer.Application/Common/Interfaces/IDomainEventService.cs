using Backend.MoneyTransfer.Domain.Commons;

namespace Backend.MoneyTransfer.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task PublishAsync(DomainEvent domainEvent);
}