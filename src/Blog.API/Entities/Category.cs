using System.ComponentModel.DataAnnotations;

namespace Blog.API.Entities;


public class Category
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }
}