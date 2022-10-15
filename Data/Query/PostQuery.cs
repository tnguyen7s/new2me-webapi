using Microsoft.EntityFrameworkCore;
using new2me_api.Models;
using System.Linq;
using System.Collections.Generic;


namespace new2me_api.Data.Query
{
    public partial class Query: IQuery
    {
        public async Task<IEnumerable<Post>> GetActivePosts(){
            var result = await this.new2meDb.Posts
                .Where(p => p.Status == Enums.PostStatusEnum.Active)
                .ToListAsync()
                .ConfigureAwait(false);

            return result;
        }

        public async Task<Post> GetPost(int id){
            var result = await this.new2meDb.Posts
                    .FindAsync(id)
                    .ConfigureAwait(false);

            return result;
        }

        public async Task<Post> CreatePost(Post post){
            post.LastUpdatedBy = 1;
            post.LastUpdatedOn = DateTime.Now;

            await this.new2meDb.Posts.AddAsync(post).ConfigureAwait(false);
            await this.new2meDb.SaveChangesAsync();
            return post;
        }

        public async Task DeletePost(Post post){
            this.new2meDb.Posts.Remove(post);
            await this.new2meDb.SaveChangesAsync();
        }

        public async Task UpdatePost(Post post){
            post.LastUpdatedBy = 1;
            post.LastUpdatedOn = DateTime.Now;

            await this.new2meDb.SaveChangesAsync();
        }

    }
}