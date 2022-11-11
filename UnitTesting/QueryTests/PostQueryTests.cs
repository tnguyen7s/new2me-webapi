using new2me_api.Enums;
using new2me_api.Models;
using Xunit;

namespace new2me_api.UnitTesting.QueryTests
{
    public class PostQueryTests: New2MeQueryTestBase
    {
        private Post[] posts;
        public PostQueryTests(){
            // seed database with data
            var user = new User{Id=0, Username="user123", Password=System.Text.Encoding.UTF8.GetBytes("userpass"), PasswordKey=System.Text.Encoding.UTF8.GetBytes("userpasskey"),Email="user123@gmail.com"};
            this.new2meDbContext.Users.Add(user);
            this.new2meDbContext.SaveChanges();


            posts = new []{
                new Post { Id = 1, Title="Chair", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.Good, Tag=PostTagEnum.Furniture, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.Active, UserId=0},
                new Post { Id = 2, Title="Desk", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.LikeNew, Tag=PostTagEnum.Furniture, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.InEditting, UserId=0},
                new Post { Id = 3, Title="Phone", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.QuiteGood, Tag=PostTagEnum.Electronics, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.Done, UserId=0},
                new Post { Id = 4, Title="Laptop", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.QuiteGood, Tag=PostTagEnum.Electronics, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.Active, UserId=0},
                new Post { Id = 5, Title="T-shirts", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.LikeNew, Tag=PostTagEnum.Apparel, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.InEditting, UserId=0},
                new Post { Id = 6, Title="Jeans", Location="123 N street, Cape Giradeau, 12345", Condition=PostConditionEnum.LikeNew, Tag=PostTagEnum.Apparel, ContactEmail="user123@gmail.com", ContactPhone="123-456-7890", Status=PostStatusEnum.Done, UserId=0}
            };
            this.new2meDbContext.Posts.AddRange(posts);

            this.new2meDbContext.SaveChanges();
        }

        [Fact]
        public void GetActivePosts_WhenHaveActivePosts_ReturnsActivePosts()
        {
            // Arrange
            var activePost1 = this.posts[0];
            var activePost2 = this.posts[3];
        
            // Act
            var postsInQueryResult = this.query.GetActivePosts().Result;
        
            // Assert
            Assert.True(postsInQueryResult.Count()==2);
            Assert.Contains(activePost1, postsInQueryResult);
            Assert.Contains(activePost2, postsInQueryResult);
        }
    }
}