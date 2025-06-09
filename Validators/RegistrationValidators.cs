using FluentValidation;
using EventMngt.DTOs;

namespace EventMngt.Validators;

public class CreateRegistrationDTOValidator : AbstractValidator<CreateRegistrationDTO>
{
    public CreateRegistrationDTOValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event ID is required");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => x.Notes != null)
            .WithMessage("Notes cannot exceed 500 characters");
    }
}

public class UpdateRegistrationDTOValidator : AbstractValidator<UpdateRegistrationDTO>
{
    public UpdateRegistrationDTOValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid registration status");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => x.Notes != null)
            .WithMessage("Notes cannot exceed 500 characters");
    }
} 