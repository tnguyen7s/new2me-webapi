using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace new2me_api.Controllers
{
    public class UserContext: IUserContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserContext(IHttpContextAccessor httpContextAccessor){
            this.httpContextAccessor = httpContextAccessor;
        }

        public int getUserID(){
            var userId = int.Parse(this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return userId;
        }
    }
}