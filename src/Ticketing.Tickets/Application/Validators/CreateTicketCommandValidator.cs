using FluentValidation;
using Ticketing.Tickets.Application.Commands;

namespace Ticketing.Tickets.Application.Validators;

public class CreateTicketCommandValidator: AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive value");
    }
}

public class UpdateTicketCommandValidator: AbstractValidator<UpdateTicketCommand>
{
    public UpdateTicketCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .GreaterThanOrEqualTo(0).WithMessage("ID must be a positive value");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive value");
    }
}