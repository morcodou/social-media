namespace Post.Query.Domain.Entities;

[ExcludeFromCodeCoverage]
[Table("Post", Schema ="dbo")]
public class PostEntity
{
    [Key]
    public Guid PostId { get; set; }
    public string Author { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime DatePosted { get; set; }
    public int Likes { get; set; }
    public virtual ICollection<CommentEntity> Comments { get; set; } = new HashSet<CommentEntity>();
}