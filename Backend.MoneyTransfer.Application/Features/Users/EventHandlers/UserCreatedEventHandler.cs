using Backend.MoneyTransfer.Application.Common.Models;
using Backend.MoneyTransfer.Domain.Events;

using MediatR;

namespace Backend.MoneyTransfer.Application.Features.Users.EventHandlers;

public class UserCreatedEventHandler : INotificationHandler<DomainEventNotification<UserCreatedEvent>>
{
    public UserCreatedEventHandler() { }

    public async Task Handle(DomainEventNotification<UserCreatedEvent> notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}