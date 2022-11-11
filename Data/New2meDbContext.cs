using Microsoft.EntityFrameworkCore;
using new2me_api.Models;

namespace new2me_api.Data
{
    public class New2meDbContext:DbContext
    {
        public New2meDbContext(DbContextOptions<New2meDbContext> options): base(options){
        }
        
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<PostPicture> PostPictures { get; set; }

    }
}