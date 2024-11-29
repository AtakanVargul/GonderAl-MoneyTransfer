using FluentValidation;

namespace Backend.MoneyTransfer.Application.Features.Transfers.Queries.TransactionById;

public class TransactionByIdQueryValidator : AbstractValidator<TransactionByIdQuery>
{
    public TransactionByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().NotEmpty();
    }
}