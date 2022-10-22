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
                .Include(post=> post.PostPictures)
                .Where(p => p.Status == Enums.PostStatusEnum.Active)
                .ToListAsync()
                .ConfigureAwait(false);

            return result;
        }

        public async Task<Post> GetPost(int id){
            var result = await this.new2meDb.Posts
                    .Include(post=> post.PostPictures)
                    .Where(post=>post.Id==id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

            return result;
        }

        public async Task<Post> CreatePost(Post post, ICollection<string> pictures, int userId){
            // Create a post
            post.UserId =userId;
            post.LastUpdatedBy = userId;
            post.LastUpdatedOn = DateTime.Now;
            await this.new2meDb.Posts.AddAsync(post).ConfigureAwait(false);

            // Create post pictures
            var postPictures = new List<PostPicture>();
            for (int i=0; i<pictures.Count; i++){
                var picture = pictures.ElementAt(i);
                var postPicture = new PostPicture{
                    Picture = Encoding.UTF32.GetBytes(picture),
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

            
            // delete pictures as well
            var oldPostPictures = await this.new2meDb.PostPictures.Where(p => p.PostId == post.Id)
                                            .ToListAsync()
                                            .ConfigureAwait(false);
            this.new2meDb.PostPictures.RemoveRange(oldPostPictures);


            await this.new2meDb.SaveChangesAsync();
        }

        public async Task UpdatePost(Post post, ICollection<String> pictures, int userId){
            // update post 
            post.LastUpdatedBy = userId;
            post.LastUpdatedOn = DateTime.Now;


            // delete previous pictures with updated pictures
            var oldPostPictures = await this.new2meDb.PostPictures.Where(p => p.PostId == post.Id)
                                            .ToListAsync()
                                            .ConfigureAwait(false);

            this.new2meDb.PostPictures.RemoveRange(oldPostPictures);
            

            for (int i=0; i<pictures.Count; i++){
                var picture = pictures.ElementAt(i);
                var postPicture = new PostPicture{
                    Picture = Encoding.UTF32.GetBytes(picture),
                    PostId = post.Id
                };
                await this.new2meDb.PostPictures.AddAsync(postPicture).ConfigureAwait(false);
            }

            // save all changes
            await this.new2meDb.SaveChangesAsync();
        }

    }
}