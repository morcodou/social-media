using System.Text.Json.Serialization;

namespace Post.Query.Domain.Entities;

[ExcludeFromCodeCoverage]
[Table("Comment", Schema = "dbo")]
public class CommentEntity
{
    [Key]
    public Guid CommentId { get; set; }

    public string Comment { get; set; } = null!;
    public string Username { get; set; } = null!;
    public DateTime CommentDate { get; set; }
    public bool Edited { get; set; }
    public Guid PostId { get; set; }

    [JsonIgnore]
    public virtual PostEntity Post { get; set; } = null!;
}