using System.Net.Mail;
using System.Net;
using InterfazUsuario.Interfaces;

namespace InterfazUsuario.Managers
{
    
    public class EmailManager : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailManager(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using var client = new SmtpClient(_config["EmailSettings:SmtpServer"],
                int.Parse(_config["EmailSettings:Port"]))
            {
                Credentials = new NetworkCredential(
                    _config["EmailSettings:Username"],
                    _config["EmailSettings:Password"]),
                EnableSsl = true
            };

            await client.SendMailAsync(
                new MailMessage(_config["EmailSettings:Username"], email, subject, message));
        }
    }


}




