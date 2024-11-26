using Backend.MoneyTransfer.Application.Common.Exceptions;
using Backend.MoneyTransfer.Application.Common.Interfaces;
using Backend.MoneyTransfer.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Backend.MoneyTransfer.Application.Features.Users.Queries.GetBalance;

public class GetBalanceQuery : IRequest<UserBalanceDto>
{
    public Guid Id { get; set; }
}

public class GetBalanceQueryHandler : IRequestHandler<GetBalanceQuery, UserBalanceDto>
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public GetBalanceQueryHandler(IRepository<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserBalanceDto> Handle(GetBalanceQuery command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.Id);

        if (user is null)
        {
            throw new NotFoundException(nameof(User), command.Id);
        }

        return _mapper.Map<UserBalanceDto>(user);
    }
}