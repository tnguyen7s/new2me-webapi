using new2me_api.Enums;

namespace new2me_api.Models

{
    public class Post
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Location { get; set; }
        public PostConditionEnum Condition { get; set; }
        public string? Description { get; set; }
        public PostTagEnum Tag { get; set; }
        public string? Picture { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public PostStatusEnum Status { get; set; }
    }
}