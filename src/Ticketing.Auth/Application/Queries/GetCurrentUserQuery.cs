using MediatR;
using Ticketing.Common.DTO;

namespace Ticketing.Auth.Application.Queries;

public record GetCurrentUserQuery: IRequest<UserPayload>;