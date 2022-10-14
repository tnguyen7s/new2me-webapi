using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace new2me_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get(){
            return new string[]{"Chair", "Desk", "Table", "Mattress"};
        }
    }
}