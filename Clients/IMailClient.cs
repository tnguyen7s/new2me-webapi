using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new2me_api.Clients
{
    public interface IMailClient
    {

        /// <summary>
        /// Send a reset password link to user's email
        /// </summary>
        /// <param name="to_"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task sendResetPassword(string to_, string token);
    }
}