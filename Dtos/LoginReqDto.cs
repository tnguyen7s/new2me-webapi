using System.ComponentModel.DataAnnotations;

namespace new2me_api.Dtos{
    
    public class LoginReqDto{
        [Required]
        public string? UsernameOrEmail { get; set; }

        [Required]
        public string? Password { get; set; }
    }   
}
