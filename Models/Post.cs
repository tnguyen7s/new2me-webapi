using System.ComponentModel.DataAnnotations;
using new2me_api.Enums;

namespace new2me_api.Models

{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public PostConditionEnum Condition { get; set; }

        public string? Description { get; set; }

        [Required]
        public PostTagEnum Tag { get; set; }

        //public string? Picture { get; set; }

        [Required]
        public string? ContactEmail { get; set; }

        [Required]
        public string? ContactPhone { get; set; }

        [Required]
        public PostStatusEnum Status { get; set; }

        public DateTime LastUpdatedOn { get; set; }
        
        public int LastUpdatedBy { get; set; }
    }
}