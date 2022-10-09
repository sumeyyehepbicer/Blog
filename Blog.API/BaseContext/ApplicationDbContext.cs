using Blog.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.BaseContext
{
    public partial class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FileMedia> FileMedias { get; set; }
        public DbSet<LikeIt> LikeIts { get; set; }


       
    }
}
