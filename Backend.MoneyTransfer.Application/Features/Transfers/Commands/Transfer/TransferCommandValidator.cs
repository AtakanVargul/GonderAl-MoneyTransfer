using FluentValidation;

namespace Backend.MoneyTransfer.Application.Features.Transfers.Commands.Transfer;

public class TransferCommandValidator : AbstractValidator<TransferCommand>
{
    public TransferCommandValidator()
    {
        RuleFor(x => x.SenderId)
            .NotNull().NotEmpty();

        RuleFor(x => x.ReceiverId)
            .NotNull().NotEmpty();

        RuleFor(x => x.Amount)
            .NotNull().NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(100);
    }
}