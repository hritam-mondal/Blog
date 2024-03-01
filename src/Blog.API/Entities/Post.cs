using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Blog.API.Entities;

public class Post
{
    [Key]
    public ObjectId Id { get; set; }
    [Required]
    public required string Title { get; set; }
    public string? ImageUrl { get; set; }
    [Required]
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    [Required]
    public required string Author { get; set; }
    public List<string>? Tags { get; set; }
    public int Views { get; set; }
}
