using MediatR;
using Ticketing.Auth.Application.DTO;

namespace Ticketing.Auth.Application.Commands;

public record SignInCommand(string Email, string Password) : IRequest<AuthResponse>;