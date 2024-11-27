using Backend.MoneyTransfer.Application.Common.Models.AccountModels;
using Backend.MoneyTransfer.Application.Features.Users.Commands.LoginUser;
using Backend.MoneyTransfer.Application.Features.Users.Commands.Register;
using Backend.MoneyTransfer.Application.Features.Users.Queries;
using Backend.MoneyTransfer.Application.Features.Users.Queries.GetBalance;

using Microsoft.AspNetCore.Mvc;

namespace Backend.MoneyTransfer.API.Controllers;

public class UserController : ApiControllerBase
{

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> RegisterAsync(RegisterCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync(LoginCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpPost("balance")]
    public async Task<ActionResult<UserBalanceDto>> Balance(GetBalanceQuery query)
    {
        return await Mediator.Send(query);
    }
}