using new2me_api.Models;

namespace new2me_api.Data.Query
{
    public interface IQuery
    {
        /// <summary>
        /// Get All Active Posts
        /// </summary>
        /// <returns>A list of active posts</returns>
        Task<IEnumerable<Post>> GetActivePosts();

        /// <summary>
        /// Get a post given the id
        /// </summary>
        /// <param name="id">Identifier of the post</param>
        /// <returns>A Post or null</returns>
        Task<Post> GetPost(int id);

        /// <summary>
        /// Create a new post in new2meDb
        /// </summary>
        /// <param name="post"></param>
        /// <returns>Return the post containning the id</returns>
        Task<Post> CreatePost(Post post);

        /// <summary>
        /// Delete a post given its id
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        Task DeletePost(Post post);

        /// <summary>
        /// Update a post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        Task UpdatePost(Post post);
    }
}