using Backend.MoneyTransfer.Application.Common.Exceptions;
using Backend.MoneyTransfer.Application.Common.Interfaces;
using Backend.MoneyTransfer.Application.Common.Pagination;
using Backend.MoneyTransfer.Application.Common.Mappings;
using Backend.MoneyTransfer.Domain.Entities;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using MediatR;

namespace Backend.MoneyTransfer.Application.Features.Transfers.Queries.AllTransactions;

public class AllTransactionsQuery : SearchQueryParams, IRequest<PaginatedList<TransactionDto>>
{

}

public class GetAllTransactionsQueryHandler : IRequestHandler<AllTransactionsQuery, PaginatedList<TransactionDto>>
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IMapper _mapper;

    public GetAllTransactionsQueryHandler(IRepository<Transaction> transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TransactionDto>> Handle(AllTransactionsQuery query, CancellationToken cancellationToken)
    {
        var transactions = _transactionRepository.GetAll() 
            ?? throw new NotFoundException(nameof(Transaction));

        return await transactions.ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(query.Page, query.Size);
    }
}