using Backend.MoneyTransfer.Application.Features.Users.Commands.LoginUser;
using Backend.MoneyTransfer.Application.Features.Users.Commands.Register;
using Backend.MoneyTransfer.Application.Features.Users.Queries.Balance;
using Backend.MoneyTransfer.Application.Common.Models.AccountModels;
using Backend.MoneyTransfer.Application.Features.Users.Queries;

using Microsoft.AspNetCore.Mvc;

namespace Backend.MoneyTransfer.API.Controllers;

public class UserController : ApiControllerBase
{

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("{id}/balance")]
    public async Task<ActionResult<BalanceResponse>> Balance([FromRoute]Guid id)
    {
        return await Mediator.Send(new BalanceQuery { Id = id });
    }
}