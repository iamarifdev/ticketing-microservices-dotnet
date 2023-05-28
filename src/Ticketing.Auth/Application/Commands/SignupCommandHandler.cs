using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using Ticketing.Auth.Application.DTO;
using Ticketing.Auth.Application.Services;
using Ticketing.Auth.Domain.Entities;
using Ticketing.Auth.Persistence.Repositories;
using Ticketing.Common.DTO;
using Ticketing.Common.Services;

namespace Ticketing.Auth.Application.Commands;

public class SignupCommandHandler : IRequestHandler<SignupCommand, SignupResponse>
{
    private readonly IOptions<JwtConfig> _jwtConfig;
    private readonly IUserRepository _userRepository;

    public SignupCommandHandler(IUserRepository userRepository, IOptions<JwtConfig> jwtConfig)
    {
        _userRepository = userRepository;
        _jwtConfig = jwtConfig;
    }

    public async Task<SignupResponse> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        var user = request.Dto.Adapt<User>();
        user.Password = await PasswordService.ToHash(user.Password);

        await _userRepository.CreateAsync(user);

        var response = user.Adapt<SignupResponse>();
        var token = GetJwtToken(response);

        return response with { Token = token };
    }

    private string GetJwtToken(SignupResponse response)
    {
        var userPayload = new UserPayload(response.Id, response.Email);
        return JwtTokenService.GenerateToken(userPayload, _jwtConfig.Value);
    }
}