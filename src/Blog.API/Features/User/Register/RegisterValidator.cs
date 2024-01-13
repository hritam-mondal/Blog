using Blog.API.Features.Authentication.Register;
using FluentValidation;

namespace Blog.API.Features.User.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Must(ContainUppercase)
                .WithMessage("Password must contain at least one uppercase letter.")
            .Must(ContainLowercase)
                .WithMessage("Password must contain at least one lowercase letter.")
            .Must(ContainDigit)
                .WithMessage("Password must contain at least one digit.")
            .Must(ContainSpecialCharacter)
                .WithMessage("Password must contain at least one special character.");
    }

    private bool ContainUppercase(string password) => password.Any(char.IsUpper);
    private bool ContainLowercase(string password) => password.Any(char.IsLower);
    private bool ContainDigit(string password) => password.Any(char.IsDigit);
    private bool ContainSpecialCharacter(string password) => password.Any(ch => !char.IsLetterOrDigit(ch));
}
