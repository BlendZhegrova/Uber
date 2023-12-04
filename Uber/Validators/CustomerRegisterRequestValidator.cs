using FluentValidation;
using Uber.Contract.V1.Requests;

namespace Uber.Validators;

public class CustomerRegisterRequestValidator : AbstractValidator<CustomerRegisterRequest>
{
    public CustomerRegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Invalid email format.");
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 25 characters.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 25 characters.");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$")
            .WithMessage("Invalid phone number format.");
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Registration date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Birth date cannot be in the future.");
    }
}