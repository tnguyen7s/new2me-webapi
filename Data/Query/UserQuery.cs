using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using new2me_api.Models;

namespace new2me_api.Data.Query
{
    public partial class Query:IQuery
    {
        public async Task<User> Authenticate(string username, string password){
            return await this.new2meDb.Users.FirstOrDefaultAsync(
                u => u.Username==username && u.Password == password
            );
        }
    }
}