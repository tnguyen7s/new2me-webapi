using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using new2me_api.Enums;

namespace new2me_api.Dtos
{
    public class PostDto
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
        [RegularExpression("[\\d]{3}-[\\d]{3}-[\\d]{4}", ErrorMessage="Phone number with incorrect format. Usage: ddd-dddd-ddd.")]
        public string? ContactPhone { get; set; }

        [Required]
        public PostStatusEnum Status { get; set; }

        [Required]
        public ICollection<string> Pictures { get; set; }
    }
}