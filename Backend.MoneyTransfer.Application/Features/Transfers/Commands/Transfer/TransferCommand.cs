using Backend.MoneyTransfer.Application.Common.Exceptions;
using Backend.MoneyTransfer.Application.Common.Interfaces;
using Backend.MoneyTransfer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MediatR;
using AutoMapper;

namespace Backend.MoneyTransfer.Application.Features.Transfers.Commands.Transfer;

public class TransferCommand : IRequest<TransactionResponse>
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
}

public class TransferCommandHandler : IRequestHandler<TransferCommand, TransactionResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IMapper _mapper;

    public TransferCommandHandler(UserManager<User> userManager,
        IRepository<User> userRepository,
        IRepository<Transaction> transactionRepository,
        IMapper mapper)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<TransactionResponse> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var sender = await _userManager.FindByIdAsync(request.SenderId.ToString());
        if (sender is null)
        {
            throw new NotFoundException(nameof(User), request.SenderId);
        }

        var receiver = await _userManager.FindByIdAsync(request.ReceiverId.ToString());
        if (receiver is null)
        {
            throw new NotFoundException(nameof(User), request.ReceiverId);
        }

        if (request.Amount > sender.Balance)
        {
            throw new InsufficientBalanceException(sender.Balance, request.Amount);
        }

        var transaction = new Transaction
        {
            User = sender,
            UserId = sender.Id,
            ReceiverId = receiver.Id,
            Amount = request.Amount,
            Description = request.Description,
            TransactionDate = DateTimeOffset.UtcNow,
            Status = Domain.Enums.TransactionStatus.Completed,
        };

        await _transactionRepository.AddAsync(transaction);

        sender.Balance -= request.Amount;
        await _userRepository.UpdateAsync(sender);

        receiver.Balance += request.Amount;
        await _userRepository.UpdateAsync(receiver);

        return _mapper.Map<TransactionResponse>(transaction);
    }
}