using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace new2me_api.Dtos
{
    public class PostContactDto
    {
        public int Id { get; set; }

        [Required]
        public string? ContactEmail { get; set; }

        [Required]
        [RegularExpression("[\\d]{3}-[\\d]{3}-[\\d]{4}", ErrorMessage="Phone number with incorrect format. Usage: ddd-dddd-ddd.")]
        public string? ContactPhone { get; set; }
    }
}