using MediatR;
using Ticketing.Auth.Application.Extensions;
using Ticketing.Auth.Application.Queries;
using Ticketing.Common.DTO;

namespace Ticketing.Auth.Application.Handlers;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserPayload>
{
    
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCurrentUserQueryHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserPayload> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.GetCurrentUser();
        return await Task.FromResult(user);
    }
}