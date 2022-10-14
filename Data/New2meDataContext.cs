using Microsoft.EntityFrameworkCore;
using new2me_api.Models;

namespace new2me_api.Data
{
    public class New2meDataContext:DbContext
    {
        public New2meDataContext(DbContextOptions<New2meDataContext> options): base(options){
        }
        
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

    }
}