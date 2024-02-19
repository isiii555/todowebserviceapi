using MimeKit;
using MailKit.Net.Smtp;
using System.Linq.Expressions;
using System.Net.Mail;
using TodoWebService.Models.DTOs.Email;

namespace TodoWebService.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _config;
        public EmailService(EmailConfig config) {
            _config = config;
        }
        public async Task SendEmailAsync(EmailRequest request)
        {
            var newEmail = new MimeMessage();
            newEmail.From.Add(new MailboxAddress(_config.DisplayName, _config.Email));
            newEmail.To.Add(new MailboxAddress("Receiver",request.ToEmail));
            newEmail.Subject = request.Subject;
            newEmail.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = request.Body
            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_config.Host,_config.Port,false);
            await smtp.AuthenticateAsync(_config.Email, _config.Password);
            await smtp.SendAsync(newEmail);
            await smtp.DisconnectAsync(true);
        }
    }
}
