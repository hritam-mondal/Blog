using FluentValidation;

namespace Blog.API.Features.Post.Create;

public class CreatePostValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255).WithMessage("Title cannot exceed 255 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("AuthorId is required.")
            .NotEqual(Guid.Empty).WithMessage("AuthorId cannot be empty.");
    }
}
