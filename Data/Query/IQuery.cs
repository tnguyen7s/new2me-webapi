using new2me_api.Dtos;
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
        /// Get posts belonging to a particular tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Task<IEnumerable<Post>> GetPostsByTag(int tag);


        /// <summary>
        /// Get all posts created by the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Post>> GetUserPosts(int userId);


        /// <summary>
        /// Create a new post in new2meDb
        /// </summary>
        /// <param name="post"></param>
        /// <param name="pictures"></param>
        /// <returns>Return the post containning the id</returns>
        Task<Post> CreatePost(Post post, ICollection<string> pictures, int userId);

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
        /// /// <param name="pictures"></param>
        /// <returns></returns>
        Task UpdatePost(Post post, ICollection<string> pictures, int userId);

        /// <summary>
        /// Authenticate a user using provided username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Authenticated user in the database</returns>
        Task<User> Authenticate(string username, string password);


        /// <summary>
        /// Signs user up
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns>The user</returns>
        Task<User> SignUp(string username, string password, string email);

        /// <summary>
        /// Check if the username already exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns>True if username already exists</returns>
        Task<bool> UsernameExists(string username);

        /// <summary>
        /// Get User by their Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User</returns>
        Task<User> GetUserByEmail(string email);

        /// <summary>
        /// Get User by their ID
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>User</returns>
        Task<User> GetUserById(int id);

        /// <summary>
        /// Update the user.
        /// </summary>
        /// <param name="user">The User entity</param>
        /// <returns></returns>
        Task UpdateUser(User user);
    }
}