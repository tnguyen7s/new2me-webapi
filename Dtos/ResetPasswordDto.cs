using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace new2me_api.Dtos
{
    public class ResetPasswordDto
    {
        [Required]
        public string Password { get; set; }
    }
}