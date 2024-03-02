namespace Blog.Client.Models;

public class Blog
{
    public string? id { get; set; }
    public string? title { get; set; }
    public string? imageUrl { get; set; }
    public string? content { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    public string? author { get; set; }
    public string? tags { get; set; }
    public int views { get; set; }
}
