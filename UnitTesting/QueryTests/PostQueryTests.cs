using new2me_api.Enums;
using new2me_api.Models;
using Xunit;
using Newtonsoft.Json;

namespace new2me_api.UnitTesting.QueryTests
{
    public class PostQueryTests: New2MeQueryTestBase
    {
        private Post[] posts;
        public PostQueryTests(){
            // seed database with dummy data
            // a user whose id=2
            var user2 = new User{Id=2, Username="user123", Password=System.Text.Encoding.UTF8.GetBytes("userpass"), PasswordKey=System.Text.Encoding.UTF8.GetBytes("userpasskey"),Email="user123@gmail.com"};
            // a user whose id=3
            var user3 = new User{Id=3, Username="user567", Password=System.Text.Encoding.UTF8.GetBytes("user2pass"), PasswordKey=System.Text.Encoding.UTF8.GetBytes("user2passkey"),Email="user567@gmail.com"};
            this.new2meDbContext.Users.Add(user2);
            this.new2meDbContext.Users.Add(user3);
            this.new2meDbContext.SaveChanges();

            // posts owned by two users
            posts = new []{
                new Post { Id = 1, Title="Chair", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.Good, Tag=PostTagEnum.Furniture, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.Active, UserId=2},
                new Post { Id = 2, Title="Desk", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.LikeNew, Tag=PostTagEnum.Furniture, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.InEditting, UserId=2},
                new Post { Id = 3, Title="Phone", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.QuiteGood, Tag=PostTagEnum.Electronics, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.Done, UserId=2},
                new Post { Id = 4, Title="Laptop", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.QuiteGood, Tag=PostTagEnum.Electronics, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.Active, UserId=2},
                new Post { Id = 5, Title="T-shirts", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.LikeNew, Tag=PostTagEnum.Apparel, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.InEditting, UserId=2},
                new Post { Id = 6, Title="Jeans", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.LikeNew, Tag=PostTagEnum.Apparel, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.Done, UserId=2},
                new Post { Id = 7, Title="Jeans", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.LikeNew, Tag=PostTagEnum.Apparel, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.Done, UserId=3}
            };
            this.new2meDbContext.Posts.AddRange(posts);
            this.new2meDbContext.SaveChanges();

            // pictures for each post above
            var postPictures = new[] {
                new PostPicture{ Id = 1, Picture = new byte[10], PostId=1},
                new PostPicture{ Id = 2, Picture = new byte[10], PostId=2},
                new PostPicture{ Id = 3, Picture = new byte[10], PostId=3},
                new PostPicture{ Id = 4, Picture = new byte[10], PostId=4},
                new PostPicture{ Id = 5, Picture = new byte[10], PostId=5},
                new PostPicture{ Id = 6, Picture = new byte[10], PostId=6},
                new PostPicture{ Id = 7, Picture = new byte[10], PostId=7}
            };
            this.new2meDbContext.PostPictures.AddRange(postPictures);
            this.new2meDbContext.SaveChanges();

        }

        [Fact]
        public void GetActivePosts_WhenHaveActivePosts_ReturnsAllActivePosts()
        {
            // Arrange
            var activePost1 = this.posts[0];
            var activePost2 = this.posts[3];
        
            // Act
            var postsInQueryResult = this.query.GetActivePosts().Result;
        
            // Assert
            Assert.True(postsInQueryResult.Count()==2); // should return all active posts
            Assert.Contains(activePost1, postsInQueryResult); // should return correct posts
            Assert.Contains(activePost2, postsInQueryResult);
            Assert.True(postsInQueryResult.ElementAt(0).PostPictures.Count()==1); // post should contains an image as set up above
            Assert.True(postsInQueryResult.ElementAt(1).PostPictures.Count()==1);
        }

        [Fact]
        public void GetPost_WhenHavePostOfTheGivenId_ShouldReturnAPost(){
            // Arrange
            var postId = 2;
            var postId2 = this.posts[1];

            // Act
            var postInQueryResult = this.query.GetPost(postId).Result;

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(postId2), JsonConvert.SerializeObject(postInQueryResult)); // the two post should be identical
            Assert.True(postInQueryResult.PostPictures.Count()==1); // the post picture should be accessable/post shoule have one picture
        }

        [Fact]
        public void GetPost_WhenHaveNoPostOfTheGivenId_ShouldReturnNull(){
            // Arrange
            var postId = 100;

            // Act
            var postInQueryResult = this.query.GetPost(postId).Result;

            // Assert
            Assert.True(postInQueryResult==null);
        }

        [Fact]
        public void GetUserPosts_WhenHappy_ShouldReturnUserPosts(){
            // Arrage 
            var userId = 3;
            var postId7 = posts[6];

            // Act 
            var postsInQueryResult = this.query.GetUserPosts(userId).Result;

            // Assert
            Assert.True(postsInQueryResult.Count()==1); //user 3 should have one post
            Assert.Equal(JsonConvert.SerializeObject(postId7), JsonConvert.SerializeObject(postsInQueryResult.ElementAt(0)));
            Assert.True(postsInQueryResult.ElementAt(0).PostPictures.Count()==1); // the post picture should be accessable/post shoule have one picture
        }

        [Fact]
        public void GetActivePostsByTag_WhenHappy_ShouldReturnActivePostsOfGivenTag(){
            // Arrange
            var tag = PostTagEnum.Electronics;
            var postId4 = posts[3];

            // Act
            var postsInQueryResult = this.query.GetActivePostsByTag((int)tag).Result;

            // Assert
            Assert.True(postsInQueryResult.Count()==1);  // only one "active" post belongs to "electronics" as set up 
            Assert.Equal(JsonConvert.SerializeObject(postId4), JsonConvert.SerializeObject(postsInQueryResult.ElementAt(0)));
            Assert.True(postsInQueryResult.ElementAt(0).PostPictures.Count()==1);
        }

        [Fact]
        public async void CreatePost_WhenHappy_ShouldCreatePost(){
            // Arrange
            var post = new Post{Id = 0, Title="Jacket", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.LikeNew, Tag=PostTagEnum.Apparel, ContactEmail="user567@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.Active, UserId=3};
            var userId = 3;
            var postPictures = new [] {""};

            // Act
            var postCreated = await this.query.CreatePost(post, postPictures, userId);
            var postId8 = await this.new2meDbContext.Posts.FindAsync(8);

            // Assert
            Assert.Equal(postCreated.Id, 8);
            Assert.Equal(JsonConvert.SerializeObject(postId8), JsonConvert.SerializeObject(postCreated));
        }

        [Fact]
        public async void DeletePost_WhenHappy_ShouldDeletePost(){
            // Arrange
            var postId1 = posts[0];

            // Act
            await this.query.DeletePost(postId1);
            var queryPostId1 = await this.query.GetPost(1);

            // Assert
            Assert.Null(queryPostId1);
        }

        [Fact]
        public async void UpdatePost_WhenUpdatePostTitle_ShouldUpdatePostTitle(){
            // Arrange
            var postId1 = posts[0];
            var userId = postId1.UserId;
            var postPictures = new []{""};

            var initialTitle = postId1.Title; // get initial title
            postId1.Title = String.Empty;

            // Act
            await this.query.UpdatePost(postId1, postPictures, userId);
            var updatedPost = await this.query.GetPost(postId1.Id);

            // Assert
            Assert.NotEqual(updatedPost.Title, initialTitle);
            Assert.Equal(updatedPost.Title, string.Empty);
        }
    }
}