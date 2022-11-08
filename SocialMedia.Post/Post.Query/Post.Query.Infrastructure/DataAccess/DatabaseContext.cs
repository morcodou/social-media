using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<PostEntity> Posts { get; set; }
        public virtual DbSet<CommentEntity> Comments { get; set; }
    }
}
