using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Ticketing.Auth.Application.Commands;
using Ticketing.Auth.Application.DTO;

namespace Ticketing.Auth.Presentation.Routes;

public static class AuthRouteHandlers
{
    public static async Task<Results<Ok<SignupResponse>, ValidationProblem>> Signup(
        IValidator<SignupRequest> validator, 
        IMediator mediator,
        SignupRequest dto)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid) return TypedResults.ValidationProblem(validationResult.ToDictionary());
        
        var result = await mediator.Send(new SignupCommand(dto));
        
        return TypedResults.Ok(result);
    }
}