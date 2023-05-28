using MediatR;
using Ticketing.Auth.Application.DTO;

namespace Ticketing.Auth.Application.Commands;

public record SignupCommand(SignupRequest Dto) : IRequest<SignupResponse>;