namespace Blog.API.Contracts;

public class CreatePostRequest
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public Guid AuthorId { get; set; }
}
