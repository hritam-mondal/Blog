using Blog.API.Features.Authentication.Register;
using Blog.API.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Blog.API.Features.User.Register;

public sealed class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResult>
{
    private readonly IValidator<RegisterCommand> _validator;
    private readonly ApplicationDbContext _context;

    public RegisterHandler(ApplicationDbContext context, IValidator<RegisterCommand> validator)
    {
        _validator = validator;
        _context = context;
    }

    public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate the command using the validator
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new RegisterResult { Success = false, ErrorMessage = "Validation failed." };
            }

            // Check if the email is already registered
            if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            {
                return new RegisterResult { Success = false, ErrorMessage = "Email is already registered." };
            }

            // Validate the password strength
            if (!IsStrongPassword(request.Password!))
            {
                return new RegisterResult { Success = false, ErrorMessage = "Password does not meet the required strength criteria." };
            }

            // Hash password
            var (hashedPassword, salt) = HashPassword(request.Password!);

            // Create a new user entity
            var user = new Entities.User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FirstName = request.FirstName!,
                LastName = request.LastName!,
                Email = request.Email!,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                Role = request.Role,
                IsVerified = false
            };

            // Save the user to the database
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // Return the user data in the result
            return new RegisterResult { Success = true, RegisteredUser = user };
        }
        catch (Exception)
        {
            return new RegisterResult { Success = false, ErrorMessage = "An error occurred during registration." };
        }
    }

    private bool IsStrongPassword(string password)
    {
        // Password must be at least 8 characters long and contain a combination of uppercase letters, lowercase letters, numbers, and special characters
        var regexPattern = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\p{P}\p{S}]).{8,}$");
        return regexPattern.IsMatch(password);
    }

    private (string Hash, string Salt) HashPassword(string password)
    {
        string salt = GenerateSalt();
        string hashedPassword = ComputeHash(password, salt);
        return (hashedPassword, salt);
    }

    private string ComputeHash(string password, string salt)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

            byte[] hashedBytes = sha256.ComputeHash(combinedBytes);
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private static string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }
}
