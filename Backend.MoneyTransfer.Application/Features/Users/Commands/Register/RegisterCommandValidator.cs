using FluentValidation;

namespace Backend.MoneyTransfer.Application.Features.Users.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotNull().MaximumLength(15)
            .WithMessage("Invalid PhoneNumber.");

        RuleFor(x => x.Password)
            .NotNull().Length(6)
            .WithMessage("Invalid password.");

        RuleFor(x => x.Email)
            .NotNull().NotEmpty();

        RuleFor(x => x.FirstName)
            .NotNull().NotEmpty().Length(100)
            .WithMessage("Invalid FirstName.");

        RuleFor(x => x.LastName)
            .NotNull().NotEmpty().Length(100)
            .WithMessage("Invalid LastName.");
    }
}