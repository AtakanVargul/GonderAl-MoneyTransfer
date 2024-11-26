using FluentValidation;

namespace Backend.MoneyTransfer.Application.Features.Users.Queries.GetBalance;

public class GetBalanceQueryValidator : AbstractValidator<GetBalanceQuery>
{
    public GetBalanceQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().NotEmpty();
    }
}