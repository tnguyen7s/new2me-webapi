using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace new2me_api.Helpers
{
    public static class SMTPService
    {
        public static void sendResetPassword(string to_, string token){
            var msg = String.Format("<a href='http://localhost:5024/api/account/updatePassword?token=%s'>Reset Password</a>", token);
            var from = "myhanh717171@gmail.com";
            var mailMessage = new MailMessage(from, to_, "New2Me, Reset Your Password", msg);

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("myhanh717171", "myhanh312");
            client.EnableSsl = true;
            client.Send(mailMessage);

            client.Dispose();
        }
    }
}