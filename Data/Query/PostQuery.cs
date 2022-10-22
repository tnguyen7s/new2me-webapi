using Microsoft.EntityFrameworkCore;
using new2me_api.Models;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

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

        public async Task<Post> CreatePost(Post post, ICollection<string> pictures){
            // Create a post
            var userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            post.UserId =userId;
            post.LastUpdatedBy = userId;
            post.LastUpdatedOn = DateTime.Now;
            await this.new2meDb.Posts.AddAsync(post).ConfigureAwait(false);

            // Create post pictures
            var postPictures = new List<PostPicture>();
            for (int i=0; i<pictures.Count; i++){
                var picture = pictures.ElementAt(i);
                var postPicture = new PostPicture{
                    Picture = Encoding.UTF8.GetBytes(picture),
                    PostId = 1 // give it a default
                };
                await this.new2meDb.PostPictures.AddAsync(postPicture).ConfigureAwait(false);
                postPictures.Add(postPicture);
            }

            // when all added, save changes
            await this.new2meDb.SaveChangesAsync();


            // change the post id for the post Pictures 
            foreach (var postPicture in postPictures){
                postPicture.PostId = post.Id;
            }
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