using Backend.MoneyTransfer.Application.Common.Exceptions;
using Backend.MoneyTransfer.Application.Common.Interfaces;
using Backend.MoneyTransfer.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Backend.MoneyTransfer.Application.Features.Transfers.Queries.TransactionById;

public class TransactionByIdQuery : IRequest<TransactionDto>
{
    public Guid Id { get; set; }
}

public class GetTransactionByIdQueryHandler : IRequestHandler<TransactionByIdQuery, TransactionDto>
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IMapper _mapper;

    public GetTransactionByIdQueryHandler(IRepository<Transaction> transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Handle(TransactionByIdQuery query, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(query.Id) 
            ?? throw new NotFoundException(nameof(Transaction), query.Id);

        return _mapper.Map<TransactionDto>(transaction);
    }
}