using FluentValidation;

namespace Backend.MoneyTransfer.Application.Features.Transfers.Queries.GetTransactionById;

public class GetTransactionByIdQueryValidator : AbstractValidator<GetTransactionByIdQuery>
{
    public GetTransactionByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().NotEmpty();
    }
}