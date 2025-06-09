using FluentValidation;
using EventMngt.DTOs;

namespace EventMngt.Validators;

public class CreateNotificationDTOValidator : AbstractValidator<CreateNotificationDTO>
{
    public CreateNotificationDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Notification title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Notification message is required")
            .MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters");
    }
}

public class UpdateNotificationDTOValidator : AbstractValidator<UpdateNotificationDTO>
{
    public UpdateNotificationDTOValidator()
    {
        RuleFor(x => x.IsRead)
            .NotNull().WithMessage("IsRead status is required");
    }
} 