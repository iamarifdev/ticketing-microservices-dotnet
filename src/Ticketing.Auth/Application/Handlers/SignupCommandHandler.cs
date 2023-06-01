using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using Ticketing.Auth.Application.Commands;
using Ticketing.Auth.Application.DTO;
using Ticketing.Auth.Application.Extensions;
using Ticketing.Auth.Application.Services;
using Ticketing.Auth.Domain.Entities;
using Ticketing.Auth.Persistence.Repositories;
using Ticketing.Common.DTO;

namespace Ticketing.Auth.Application.Handlers;

public class SignupCommandHandler : IRequestHandler<SignupCommand, AuthResponse>
{
    private readonly IOptions<JwtConfig> _jwtConfig;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SignupCommandHandler(
        IUserRepository userRepository, 
        IOptions<JwtConfig> jwtConfig,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _jwtConfig = jwtConfig;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AuthResponse> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        var user = request.Dto.Adapt<User>();
        user.Password = await PasswordService.ToHash(user.Password);

        await _userRepository.CreateAsync(user);
        var response = user.Adapt<AuthResponse>();
        
        return response
            .WithJwtToken(_jwtConfig.Value)
            .SetJwtTokenCookie(_httpContextAccessor);
    }
}