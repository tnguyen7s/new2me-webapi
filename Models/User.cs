using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new2me_api.Models
{
    public class User
    {
        public long Id { get; set; }
        public string? Username { get; set; }
        public string? PhoneNum { get; set; }
        public string? Address { get; set; }
        public string? NameOfUser { get; set; }
        public string? Email { get; set;}
    }
}