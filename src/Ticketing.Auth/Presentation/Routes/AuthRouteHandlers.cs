using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Ticketing.Auth.Application.Commands;
using Ticketing.Auth.Application.DTO;
using Ticketing.Auth.Application.Queries;
using Ticketing.Common.DTO;

namespace Ticketing.Auth.Presentation.Routes;

public static class AuthRouteHandlers
{
    public static async Task<Ok<UserPayload>> GetCurrentUser(IMediator mediator)
    {
        var result = await mediator.Send(new GetCurrentUserQuery());
        
        return TypedResults.Ok(result);
    }
    
    public static async Task<Results<Ok<AuthResponse>, ValidationProblem>> Signup(
        IValidator<SignupRequest> validator, 
        IMediator mediator,
        SignupRequest dto)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid) return TypedResults.ValidationProblem(validationResult.ToDictionary());
        
        var result = await mediator.Send(new SignupCommand(dto));
        
        return TypedResults.Ok(result);
    }
    
    public static async Task<Results<Ok<AuthResponse>, ValidationProblem>> SignIn(
        IValidator<SignInCommand> validator, 
        IMediator mediator,
        SignInCommand signInCommand)
    {
        var validationResult = await validator.ValidateAsync(signInCommand);
        if (!validationResult.IsValid) return TypedResults.ValidationProblem(validationResult.ToDictionary());
        
        var result = await mediator.Send(signInCommand);
        
        return TypedResults.Ok(result);
    }
    
    public static async Task<Ok<SignOutResponse>> SignOut(IMediator mediator, SignOutCommand signOutCommand)
    {
        var result = await mediator.Send(signOutCommand);
        
        return TypedResults.Ok(result);
    }
}