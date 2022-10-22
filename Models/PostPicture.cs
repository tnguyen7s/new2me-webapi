using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace new2me_api.Models
{
    public class PostPicture
    {
        
        public int Id { get; set; }

        public byte[] Picture { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
    }
}