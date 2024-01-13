using System.ComponentModel.DataAnnotations;

namespace Blog.API.Entities;

public class Post
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public required string Title { get; set; }
    [Required]
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    [Required]
    public required User? Author { get; set; }
}
