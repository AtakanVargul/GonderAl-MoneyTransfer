using FluentValidation;

namespace Backend.MoneyTransfer.Application.Features.Users.Queries.Balance;

public class BalanceQueryValidator : AbstractValidator<BalanceQuery>
{
    public BalanceQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().NotEmpty();
    }
}