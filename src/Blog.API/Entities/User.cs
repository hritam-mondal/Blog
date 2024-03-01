using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Blog.API.Entities;

public class User
{
    [Key]
    public ObjectId Id { get; set; }
    [Required]
    [StringLength(50)]
    public required string FirstName { get; set; }
    [Required]
    [StringLength(50)]
    public required string LastName { get; set; }
    [Required]
    [StringLength(100)]
    public required string Email { get; set; }
    [Required]
    [StringLength(255)]
    public required string PasswordHash { get; set; }
    [Required]
    [StringLength(255)]
    public required string PasswordSalt { get; set; }
    [StringLength(20)]
    public string? Role { get; set; }
    public bool? IsVerified { get; set; }
}
