using MediatR;
using Ticketing.Auth.Application.DTO;

namespace Ticketing.Auth.Application.Commands;

public record SignOutCommand : IRequest<SignOutResponse>;