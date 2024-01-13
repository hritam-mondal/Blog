namespace Blog.API.Features.Post.Create;

public class CreatePostResult
{
    public bool Success { get; set; }
    public string? ErrorCode { get; set; }
    public Entities.Post? CreatedPost { get; set; }
}
