using System.ComponentModel.DataAnnotations;

namespace new2me_api.Dtos{
    
    public class LoginReqDto{
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }   
}
