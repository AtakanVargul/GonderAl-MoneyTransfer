using Backend.MoneyTransfer.Application.Common.Models.AccountModels;
using Backend.MoneyTransfer.Application.Common.Exceptions;
using Backend.MoneyTransfer.Application.Common.Interfaces;
using Backend.MoneyTransfer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MediatR;

namespace Backend.MoneyTransfer.Application.Features.Users.Commands.LoginUser;

public class LoginCommand : IRequest<LoginResponse>
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtHelper _jwtHelper;

    public LoginCommandHandler(UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtHelper jwtHelper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtHelper = jwtHelper;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(request.PhoneNumber, request.Password, false, false);

        var user = await _userManager.FindByNameAsync(request.PhoneNumber);

        await ValidateSignInResult(result);

        var credentials = await _jwtHelper.GenerateJwtTokenAsync(user);

        await _signInManager.SignInAsync(user, false);

        return new LoginResponse
        {
            Token = credentials
        };
    }

    private static async Task ValidateSignInResult(SignInResult result)
    {
        if (!result.Succeeded)
        {
            throw new LoginFailedException();
        }

        await Task.FromResult(true);
    }
}