using FluentValidation;
using EventMngt.DTOs;

namespace EventMngt.Validators;

public class CreateEventDTOValidator : AbstractValidator<CreateEventDTO>
{
    public CreateEventDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Title is required and must not exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000)
            .WithMessage("Description is required and must not exceed 1000 characters");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Start date must be in the future");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date");

        RuleFor(x => x.Location)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Location is required and must not exceed 200 characters");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID is required");

        RuleFor(x => x.OrganizerId)
            .NotEmpty()
            .WithMessage("Organizer ID is required");
    }
}

public class UpdateEventDTOValidator : AbstractValidator<UpdateEventDTO>
{
    public UpdateEventDTOValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(100)
            .When(x => x.Title != null)
            .WithMessage("Title must not exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description != null)
            .WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.StartDate.HasValue)
            .WithMessage("Start date must be in the future");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate ?? DateTime.MinValue)
            .When(x => x.EndDate.HasValue && x.StartDate.HasValue)
            .WithMessage("End date must be after start date");

        RuleFor(x => x.Location)
            .MaximumLength(200)
            .When(x => x.Location != null)
            .WithMessage("Location must not exceed 200 characters");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .When(x => x.Capacity.HasValue)
            .WithMessage("Capacity must be greater than 0");
    }
} 