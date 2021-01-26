using Identity.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Identity.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;
        private EmailSettings _emailSettings;
        private string _hostUrl;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
            _hostUrl = configuration.GetValue<string>("Domain");
            
        }

        public void SendRegistrationEmail(string email, string userId, string token)
        {
            var message = new MimeMessage();
            message.Subject = "Account Confirmation";
            message.Body = new BodyBuilder { TextBody = $"Please confirm your account by clicking on {_hostUrl}/accounts/confirm?userId={HttpUtility.UrlEncode(userId)}&token={HttpUtility.UrlEncode(token)}" }.ToMessageBody();
            message.To.Add(new MailboxAddress(email));
            message.From.Add(new MailboxAddress("no-reply"));

            SendEmail(message);
        }

        public void SendResetPasswordEmail(string email, string userId, string token)
        {
            var message = new MimeMessage();
            message.Subject = "Reset Password";
            message.Body = new BodyBuilder { TextBody = $"You have requested to reset your password. Please click following link to do it. {_hostUrl}/resetpassword?userId={HttpUtility.UrlEncode(userId)}&token={HttpUtility.UrlEncode(token)}" }.ToMessageBody();
            message.To.Add(new MailboxAddress(email));
            message.From.Add(new MailboxAddress("no-reply"));

            SendEmail(message);
        }

        private void SendEmail(MimeMessage message)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Connect(_emailSettings.SMTPHost, _emailSettings.SMTPPort, useSsl: true);
                smtpClient.Authenticate(_emailSettings.UserName, _emailSettings.Password);
                smtpClient.Send(message);
                smtpClient.Disconnect(true);
            }
        }
    }
}
