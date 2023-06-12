using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using Ticketing.Auth.Application.Commands;
using Ticketing.Auth.Application.DTO;
using Ticketing.Auth.Application.Extensions;
using Ticketing.Auth.Application.Services;
using Ticketing.Auth.Persistence.Repositories;
using Ticketing.Common.DTO;
using Ticketing.Common.Exceptions;

namespace Ticketing.Auth.Application.Handlers;

public class SignInCommandHandler : IRequestHandler<SignInCommand, AuthResponse>
{
    private readonly IOptions<JwtConfig> _jwtConfig;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SignInCommandHandler(
        IUserRepository userRepository, 
        IOptions<JwtConfig> jwtConfig, 
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _jwtConfig = jwtConfig;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AuthResponse> Handle(SignInCommand dto, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email.Trim());

        if (user is null || !await PasswordService.VerifyHash(user.Password, dto.Password))
            throw new UnauthorizedException("Invalid credentials");

        var response = user.Adapt<AuthResponse>();
        
        return response
            .WithJwtToken(_jwtConfig.Value)
            .SetJwtTokenCookie(_httpContextAccessor);
    }
}