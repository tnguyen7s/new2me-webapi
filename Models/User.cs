using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace new2me_api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public byte[] Password { get; set; }

        public byte[] PasswordKey { get; set; }


        [Required]
        public string? Email { get; set;}

        public string? PhoneNum { get; set; }

        public string? Address { get; set; }
        
        public string? NameOfUser { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}