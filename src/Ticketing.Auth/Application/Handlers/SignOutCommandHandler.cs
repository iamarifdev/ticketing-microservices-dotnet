using MediatR;
using Ticketing.Auth.Application.Commands;
using Ticketing.Auth.Application.DTO;
using Ticketing.Auth.Application.Extensions;

namespace Ticketing.Auth.Application.Handlers;

public class SignOutCommandHandler : IRequestHandler<SignOutCommand, SignOutResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SignOutCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<SignOutResponse> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        _httpContextAccessor.RemoveJwtTokenCookie();

        return await Task.Run(() => new SignOutResponse(true, null), cancellationToken);
    }
}