using FluentValidation;
using Ticketing.Auth.Application.DTO;

namespace Ticketing.Auth.Application.Validators;

public class SignupCommandValidator : AbstractValidator<SignupRequest>
{
    public SignupCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(20).WithMessage("Password must not exceed 20 characters.")
            .MinimumLength(4).WithMessage("Password must be at least 4 characters.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(25).WithMessage("First Name must not exceed 25 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(25).WithMessage("Last Name must not exceed 25 characters.");
    }
}