using Backend.MoneyTransfer.Application.Common.Mappings;
using Backend.MoneyTransfer.Domain.Entities;

namespace Backend.MoneyTransfer.Application.Features.Users.Queries;

public class UserBalanceDto : IMapFrom<User>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; set; }
    public string LastModifiedBy { get; set; }
}