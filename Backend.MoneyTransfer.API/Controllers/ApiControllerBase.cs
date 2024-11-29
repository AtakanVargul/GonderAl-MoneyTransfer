using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Backend.MoneyTransfer.API.Controllers;

[ApiController]
[Route("v1/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected ISender Mediator =>
        HttpContext.RequestServices.GetRequiredService<ISender>();
}