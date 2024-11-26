using Backend.MoneyTransfer.Application.Common.Exceptions;
using Backend.MoneyTransfer.Application.Common.Interfaces;
using Backend.MoneyTransfer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MediatR;

namespace Backend.MoneyTransfer.Application.Features.Transfers.Queries.GetAllTransactions;

public class GetAllTransactionsQuery : IRequest<IEnumerable<TransactionDto>>
{

}

public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, IEnumerable<TransactionDto>>
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IMapper _mapper;

    public GetAllTransactionsQueryHandler(IRepository<Transaction> transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransactionDto>> Handle(GetAllTransactionsQuery query, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetAll()?.ToListAsync(cancellationToken);

        if (transactions is null)
        {
            throw new NotFoundException(nameof(Transaction));
        }

        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }
}