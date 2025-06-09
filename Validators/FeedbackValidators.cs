using FluentValidation;
using EventMngt.DTOs;

namespace EventMngt.Validators;

public class CreateFeedbackDTOValidator : AbstractValidator<CreateFeedbackDTO>
{
    public CreateFeedbackDTOValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .MaximumLength(1000)
            .WithMessage("Comment is required and must not exceed 1000 characters");
    }
}

public class UpdateFeedbackDTOValidator : AbstractValidator<UpdateFeedbackDTO>
{
    public UpdateFeedbackDTOValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .MaximumLength(1000)
            .WithMessage("Comment is required and must not exceed 1000 characters");
    }
} 