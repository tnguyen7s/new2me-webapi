using MimeKit;
using new2me_api.Clients;

namespace new2me_api.Helpers
{
    public class MailClient: IMailClient
    {
        private readonly String from;
        private readonly String smtpServer;
        private readonly int port;
        private readonly String username;
        private readonly String password;

         public MailClient(IConfiguration configuration){
            this.from = configuration.GetSection("EmailConfiguration:From").Value;
            this.smtpServer = configuration.GetSection("EmailConfiguration:SmtpServer").Value;
            var port = configuration.GetSection("EmailConfiguration:Port").Value;
            this.port = int.Parse(port);
            this.username = configuration.GetSection("EmailConfiguration:Username").Value;
            this.password = configuration.GetSection("EmailConfiguration:Password").Value;
        }

        public async Task sendResetPassword(string to_, string token){
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(String.Empty, this.from));
            emailMessage.To.Add(new MailboxAddress(String.Empty, to_));
            emailMessage.Subject = "New2Me - Reset Password Link";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html){
                Text= String.Format("<p>Hi,</p><p>You have requested a password reset email.</p><p></p> <p>Please visit the following URL:</p> <a href=\"{0}\">Reset Password Link</a> <p>This link will expire in 24 hours.</p>", token)
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient()){
                try{
                    await client.ConnectAsync(this.smtpServer, this.port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(this.username, this.password);

                    await client.SendAsync(emailMessage);
                }
                catch (Exception e){
                    throw;
                }
                finally{
                    client.Dispose();
                }
            }
        }
    }
}