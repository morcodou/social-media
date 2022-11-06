using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities
{
    [Table("Post")]
    public class PostEntity
    {
        [Key]
        public Guid PostId { get; set; }
        public string Author { get; set; } = null!;
        public string Messages { get; set; } = null!;
        public DateTime DatePosted { get; set; }
        public int Likes { get; set; }
        public virtual ICollection<CommentEntity> comments { get; set; } = new HashSet<CommentEntity> ();
    }
}