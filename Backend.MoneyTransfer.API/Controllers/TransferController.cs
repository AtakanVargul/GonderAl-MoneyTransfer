using Microsoft.AspNetCore.Mvc;
using Backend.MoneyTransfer.Application.Common.Pagination;
using Backend.MoneyTransfer.Application.Features.Transfers.Queries;
using Backend.MoneyTransfer.Application.Features.Transfers.Commands;
using Backend.MoneyTransfer.Application.Features.Transfers.Commands.Transfer;
using Backend.MoneyTransfer.Application.Features.Transfers.Queries.GetAllTransactions;
using Backend.MoneyTransfer.Application.Features.Transfers.Queries.GetTransactionById;

namespace Backend.MoneyTransfer.API.Controllers;

public class TransferController : ApiControllerBase
{

    [HttpPost("transfer")]
    public async Task<TransactionResponse> Transfer([FromBody] TransferCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet]
    public async Task<PaginatedList<TransactionDto>> GetAllTransactions(GetAllTransactionsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{Id}")]
    public async Task<TransactionDto> GetTransactionById(Guid id)
    {
        return await Mediator.Send(new GetTransactionByIdQuery { Id = id });
    }
}