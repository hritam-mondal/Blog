using Blog.API.Features.User.Login;
using Blog.API.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Blog.API.Features.Authentication.Login;

public sealed class LoginHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IValidator<LoginCommand> _validator;
    private readonly ApplicationDbContext _context;

    public LoginHandler(ApplicationDbContext context, IValidator<LoginCommand> validator)
    {
        _validator = validator;
        _context = context;
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate the command using the validator
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new LoginResult { Success = false, ErrorMessage = "Validation failed." };
            }

            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (user == null)
            {
                return new LoginResult { Success = false, ErrorMessage = "User not found." };
            }

            // Verify the password
            if (!VerifyPassword(request.Password!, user.PasswordHash, user.PasswordSalt))
            {
                return new LoginResult { Success = false, ErrorMessage = "Invalid password." };
            }

            // Generate and return a JWT token for authentication
            return new LoginResult { Success = true, Token = CreateToken(user) };
        }
        catch (Exception ex)
        {
            return new LoginResult { Success = false, ErrorMessage = "An error occurred during login." };
        }
    }

    private bool VerifyPassword(string password, string hashedPassword, string salt)
    {
        // Verify the entered password against the stored hash and salt
        string hashedInput = ComputeHash(password, salt);
        return hashedInput == hashedPassword;
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

    public string CreateToken(Entities.User user)
    {
        const string keyToken = "0Qz41JQPcavsor2hipcY2rxnvL12SY2yyL7nCpujGb4=";
        string name = $"{user.FirstName} {user.LastName}";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, name),
            new Claim(ClaimTypes.Role, user.Role!),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("UserId", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyToken));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(claims: claims,
            expires: DateTime.UtcNow.AddDays(15),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    public string GenerateRandomKey(int byteLength)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var keyBytes = new byte[byteLength];
            rng.GetBytes(keyBytes);
            return Convert.ToBase64String(keyBytes);
        }
    }
}
